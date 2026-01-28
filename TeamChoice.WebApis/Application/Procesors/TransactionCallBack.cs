using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Configuration;

namespace TeamChoice.WebApis.Application.Procesors
{
    public interface ITransactionCallBack
    {
        Task<TransactionStatusDTO> CreateTransactionAsync(TransactionStatusCallback callbackPayload);
    }

    public class TransactionCallBack : ITransactionCallBack
    {
        private readonly HttpClient _httpClient;
        private readonly ClientUrlProperties _clientUrlProperties;
        private readonly ILogger<TransactionCallBack> _logger;

        public TransactionCallBack(
            HttpClient httpClient,
            ClientUrlProperties clientUrlProperties,
            ILogger<TransactionCallBack> logger)
        {
            _httpClient = httpClient;
            _clientUrlProperties = clientUrlProperties;
            _logger = logger;
        }

        public async Task<TransactionStatusDTO> CreateTransactionAsync(TransactionStatusCallback callbackPayload)
        {
            _logger.LogInformation("📨 Creating callback for transactionId={Id} at {Time}",
                callbackPayload.TransactionId, callbackPayload.TransactionTimestamp);

            // Matches: clientUrlProperties.getPaymentReportUrlCallBack()
            string callbackUrl = _clientUrlProperties.PaymentReportUrlCallBack;

            try
            {
                var response = await _httpClient.PostAsJsonAsync(callbackUrl, callbackPayload);

                if (!response.IsSuccessStatusCode)
                {
                    // Matches handleErrorResponse logic
                    var errorBody = await response.Content.ReadAsStringAsync();
                    string errorMsg = $"❌ Callback failed with status [{(int)response.StatusCode}]: {errorBody}";
                    _logger.LogError(errorMsg);
                    throw new HttpRequestException(errorMsg);
                }

                // Matches doOnSuccess logic
                _logger.LogInformation("✅ Callback successfully sent to {Url}", callbackUrl);

                var result = await response.Content.ReadFromJsonAsync<TransactionStatusDTO>();
                return result;
            }
            catch (Exception err)
            {
                // Matches doOnError logic
                _logger.LogError(err, "❌ Error during callback transmission: {Message}", err.Message);
                throw;
            }
        }
    }
}
