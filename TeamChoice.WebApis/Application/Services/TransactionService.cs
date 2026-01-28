using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Application.Mappers;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Application.Services
{
    public interface ITransactionService
    {
        Task<RemittanceResultDTO> ValidateAndSaveTransactionAsync(TransactionRequestDto requestDTO, string serviceType);
    }

    public class TransactionService : ITransactionService
    {
        public const string STATUS_REM = "REM";

        private readonly IRemittanceService _remittanceService;
        private readonly IAgentTransactionFacade _agentTransactionFacade;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
            IRemittanceService remittanceService,
            IAgentTransactionFacade agentTransactionFacade,
            ILogger<TransactionService> logger)
        {
            _remittanceService = remittanceService;
            _agentTransactionFacade = agentTransactionFacade;
            _logger = logger;
        }

        public async Task<RemittanceResultDTO> ValidateAndSaveTransactionAsync(TransactionRequestDto requestDTO, string serviceType)
        {
            _logger.LogInformation("🔍 Validating transaction request: {@Request}", requestDTO);

            if (requestDTO == null || string.IsNullOrWhiteSpace(requestDTO.PartnerReference))
            {
                _logger.LogWarning("Invalid transaction request: request or partner reference is null/empty");
                throw new ArgumentException("Transaction request or partner reference must not be empty");
            }

            string locationId = requestDTO.SendingLocation?.LocationId;
            if (!TransactionUtil.ValidateSenderLocation(locationId))
            {
                _logger.LogWarning("Invalid sender location: {LocationId}", locationId);
                // Throwing exception to be handled by global middleware or orchestrator
                throw new InvalidOperationException($"Invalid sender location ID: {locationId}");
            }

            if (requestDTO.Payment != null)
            {
                requestDTO.Payment.ServiceCode = serviceType;
            }

            try
            {
                // Step 1: Map to internal models
                var transaction = TransactionMapperUtil.ToTransaction(requestDTO);
                _logger.LogInformation("Mapped to internal transaction: {@Transaction}", transaction);

                var smtTransaction = TransactionSmtMapperUtil.ToSmtTransaction(transaction);
                _logger.LogInformation("Mapped to SMT transaction: {@SmtTransaction}", smtTransaction);

                // Step 2: Set status metadata
                smtTransaction.TrnsStatus = STATUS_REM;
                smtTransaction.TrnsSubStatus = "P";
                smtTransaction.ActualStatus = "P";

                // Step 3: Save and forward
                return await SaveAndSendRemittanceAsync(smtTransaction);
            }
            catch (Exception error)
            {
                // In Java: .doOnError(error -> log.error(...))
                // We log and rethrow here
                _logger.LogError(error, "❌ Failed processing transaction [{Ref}]: {Message}", requestDTO.PartnerReference, error.Message);
                throw;
            }
        }

        private async Task<RemittanceResultDTO> SaveAndSendRemittanceAsync(SmtTransaction smtTransaction)
        {
            _logger.LogInformation("Converting to remittance request: {@SmtTransaction}", smtTransaction);
            var remittanceRequest = RemittanceMapperUtil.ToRemittanceRequest(smtTransaction);
            _logger.LogInformation("Remittance request prepared: {@RemittanceRequest}", remittanceRequest);

            // var payment = remittanceRequest.PaymentMode; // unused in original code snippet logic, but retrieved

            var response = await _remittanceService.SendRemittanceAsync(remittanceRequest);

            if (!string.IsNullOrWhiteSpace(response.Reference))
            {
                _logger.LogInformation("✅ Remittance sent successfully with reference: {Reference}", response.Reference);
            }
            else
            {
                _logger.LogWarning("Remittance response returned with empty reference");
            }

            return response;
        }
    }
}
