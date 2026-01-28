using System.Text.RegularExpressions;
using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Application.Procesors;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Application.Validators;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Application.Orchestrators;

public interface ITransactionOrchestrator
{
    Task<RemittanceResultDTO> HandleAsync(TransactionRequestDto requestDTO);
}


    public class TransactionOrchestrator : ITransactionOrchestrator
    {
        private readonly IAgentTransactionFacade _agentTransactionFacade;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionForwarder _transactionForwarder;
        private readonly ITransactionCallBack _transactionCallBack;
        private readonly ITransactionValidator _transactionValidator;
        private readonly ITransactionProcessor _transactionProcessor;
        private readonly ILogger<TransactionOrchestrator> _logger;

        public TransactionOrchestrator(
            IAgentTransactionFacade agentTransactionFacade,
            ITransactionService transactionService,
            ITransactionForwarder transactionForwarder,
            ITransactionCallBack transactionCallBack,
            ITransactionValidator transactionValidator,
            ITransactionProcessor transactionProcessor,
            ILogger<TransactionOrchestrator> logger)
        {
            _agentTransactionFacade = agentTransactionFacade;
            _transactionService = transactionService;
            _transactionForwarder = transactionForwarder;
            _transactionCallBack = transactionCallBack;
            _transactionValidator = transactionValidator;
            _transactionProcessor = transactionProcessor;
            _logger = logger;
        }

        public async Task<RemittanceResultDTO> HandleAsync(TransactionRequestDto requestDTO)
        {
            try
            {
                // Validate Sender Phone
                 _transactionValidator.Validate(requestDTO);

                // Validate Unique Ref
                await ValidateUniqueRefAsync(requestDTO.PartnerReference);

                // Resolve Service Type
                string serviceType = await ResolveServiceTypeAsync(requestDTO);

                _logger.LogInformation("Processing transaction for Ref: {Ref}", requestDTO.PartnerReference);

                // Save Transaction (Returns RemittanceResultDTO)
                var result = await _transactionService.ValidateAndSaveTransactionAsync(requestDTO, serviceType);

                // Post-Save Logic: Prepare Data
                requestDTO.TransactionId= result.Reference;

                var partnerTx = _transactionProcessor.BuildPartnerTransaction(requestDTO);
                var dtoCopy = MapToTransactionRequestDTOCopy(requestDTO);

                // 1. Insert Partner Transaction Log
                await _agentTransactionFacade.InsertPartnerTransactionAsync(partnerTx);

                // 2. Find internal transaction code
                var trnsCode = await _agentTransactionFacade.FindTrnsCodeFromSmtTransactionsAsync(result.Reference);

                if (string.IsNullOrEmpty(trnsCode))
                {
                    throw new InvalidOperationException("Transaction not found in SMT");
                }

                // 3. Forward to provider and send callback
                await ForwardAndCallbackAsync(dtoCopy, trnsCode);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "❌ Transaction handling failed: {Message}", e.Message);
                throw; // Propagate exception to controller
            }
        }

        private async Task<string> ResolveServiceTypeAsync(TransactionRequestDto requestDTO)
        {
            string serviceCode = requestDTO.Payment?.ServiceCode?.Trim() ?? "";

            // Check if 6 digits
            if (Regex.IsMatch(serviceCode, @"^\d{6}$"))
            {
                _logger.LogInformation("🔍 Resolving 6-digit serviceCode via AgentTransactionFacade: {ServiceCode}", serviceCode);
                var resolvedCode = await _agentTransactionFacade.FindServiceCodeUsingBankServiceTypeAsync(serviceCode);

                if (string.IsNullOrEmpty(resolvedCode))
                {
                    throw new ArgumentException($"Service type not found for code: {serviceCode}");
                }
                return resolvedCode;
            }
            else
            {
                // For non-6-digit values, assume it's already a service type string.
                // Trigger best-effort LocId resolution (fire-and-forget logic awaited here for simplicity, or strictly fire-and-forget using Task.Run)
                try
                {
                    var locId = await _agentTransactionFacade.GetLocIdForCashPickupServiceAsync(serviceCode);
                    _logger.LogDebug("📍 Resolved LocId for serviceCode {ServiceCode}: {LocId}", serviceCode, locId);
                }
                catch (Exception err)
                {
                    _logger.LogWarning("⚠️ Unable to resolve LocId for serviceCode {ServiceCode}: {Message}", serviceCode, err.Message);
                }

                return serviceCode;
            }
        }

        private async Task ValidateUniqueRefAsync(string partnerReference)
        {
            string refStr = partnerReference?.Trim() ?? "";
            if (string.IsNullOrEmpty(refStr))
            {
                _logger.LogWarning("⚠️ Partner reference is blank; skipping duplicate validation");
                return;
            }

            var result = await _agentTransactionFacade.ValidateTransactionAsync(refStr);
            string normalized = result?.Trim() ?? "";

            _logger.LogDebug("🔎 validateTransaction returned: '{Normalized}' for partnerReference: '{Ref}'", normalized, refStr);

            if (!string.IsNullOrEmpty(normalized) && !"NOT_FOUND".Equals(normalized, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("⚠️ Duplicate partner reference detected for '{Ref}': facade returned '{Normalized}'", refStr, normalized);
                throw new InvalidOperationException("Duplicate partner reference");
            }
        }

        private async Task ForwardAndCallbackAsync(TransactionRequestDTOCopy copy, string trnsCode)
        {
            var fwd = await _transactionForwarder.CreateTransactionAsync(copy, trnsCode);

            if (fwd != null)
            {
                var cb = new TransactionStatusCallback
                {
                    TransactionTimestamp = fwd.TransactionTimestamp,
                    TransactionId = fwd.TransactionId,
                    Status = fwd.Status
                };
                await _transactionCallBack.CreateTransactionAsync(cb);
            }
        }

        private TransactionRequestDTOCopy MapToTransactionRequestDTOCopy(TransactionRequestDto source)
        {
            // Simple mapping using JSON serialization to deep copy/map similar structures
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(source);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TransactionRequestDTOCopy>(json);
        }
    }


public sealed record TransactionOrchestrationResult(string Reference);