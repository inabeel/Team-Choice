using System.Net;
using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Application.Mappers;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Configuration;

namespace TeamChoice.WebApis.Application.Procesors
{
    public interface ITransactionForwarder
    {
        Task<TransactionStatusDTO> CreateTransactionAsync(TransactionRequestDTOCopy transactionDTO, string trnsCode);
    }

    public class TransactionForwarder : ITransactionForwarder
    {
        private readonly HttpClient _httpClient;
        private readonly IAgentTransactionFacade _agentTransactionFacade;
        private readonly ClientUrlProperties _clientUrlProperties;
        private readonly ILogger<TransactionForwarder> _logger;

        public TransactionForwarder(
            HttpClient httpClient,
            IAgentTransactionFacade agentTransactionFacade,
            ClientUrlProperties clientUrlProperties,
            ILogger<TransactionForwarder> logger)
        {
            _httpClient = httpClient;
            _agentTransactionFacade = agentTransactionFacade;
            _clientUrlProperties = clientUrlProperties;
            _logger = logger;
        }

        public async Task<TransactionStatusDTO> CreateTransactionAsync(TransactionRequestDTOCopy transactionDTO, string trnsCode)
        {
            string logServiceCode = transactionDTO.Payment?.ServiceCode;
            _logger.LogInformation("🚀 Initiating transaction forwarding for transactionCode={TrnsCode} and partnerRef={Ref}", trnsCode, logServiceCode);

            // Validation: if service code is "00001", return empty TransactionStatusDTO without forwarding
            string serviceCode = transactionDTO.Payment?.ServiceCode;
            if ("00001".Equals(serviceCode))
            {
                _logger.LogWarning("Service code is 00001; creating cash transaction and returning pending.");
                return new TransactionStatusDTO();
            }

            try
            {
                // Execute dependency calls sequentially (replacing nested flatMap)
                var smtComm = await _agentTransactionFacade.FindSmtCommissionAsync(trnsCode);
                var outbound = await _agentTransactionFacade.FindOutboundCredAsync(serviceCode);

                // Assuming CodeMapping.CODE_00006 maps to "00006"
                var rcvComm = await _agentTransactionFacade.FindRcvcomByAgtCodeAsync("00006");

                var mappedRequest = TransactionPayloadMapper.ToTransactionRequest(
                    transactionDTO,
                    rcvComm,
                    trnsCode,
                    smtComm,
                    outbound,
                    _clientUrlProperties.CallbackUrl
                );

                _logger.LogInformation("📤 Forwarding transaction to external provider...");
                _logger.LogInformation("Request payload: {@MappedRequest}", mappedRequest);

                var response = await _httpClient.PostAsJsonAsync(_clientUrlProperties.PaymentReportUrl, mappedRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    string errorMsg = $"❌ Failed to send transaction to SmartRemit [{response.StatusCode}]: {body}";
                    _logger.LogError(errorMsg);

                    // Throw custom or standard exception to be handled by caller/middleware
                    throw new HttpRequestException(errorMsg, null, HttpStatusCode.ServiceUnavailable);
                }

                var result = await response.Content.ReadFromJsonAsync<TransactionStatusDTO>();

                _logger.LogInformation("✅ Transaction sent successfully to {Url}", _clientUrlProperties.PaymentReportUrl);

                return result;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "❌ Failed to send transaction: {Message}", err.Message);
                throw;
            }
        }
    }
}
