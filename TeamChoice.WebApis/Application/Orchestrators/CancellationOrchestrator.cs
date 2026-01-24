using TeamChoice.WebApis.Application.Facades;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Domain.Configuration;
using TeamChoice.WebApis.Domain.Exceptions;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Application.Orchestrators;

public interface ICancellationOrchestrator
{
    Task<CancelTransactionResult> ExecuteAsync(CancelTransactionDto request);
}

/// <summary>
/// Coordinates transaction cancellation workflow:
/// - Validate transaction existence
/// - Validate cancellable state
/// - Call cancellation service
/// </summary>
public sealed class CancellationOrchestrator : ICancellationOrchestrator
{
    private readonly ICancellationService _cancellationService;
    private readonly IAgentTransactionFacade _agentTransactionFacade;
    private readonly ClientUrlProperties _clientProps;
    private readonly ILogger<CancellationOrchestrator> _logger;

    public CancellationOrchestrator(
        ICancellationService cancellationService,
        IAgentTransactionFacade agentTransactionFacade,
        ClientUrlProperties clientProps,
        ILogger<CancellationOrchestrator> logger)
    {
        _cancellationService = cancellationService;
        _agentTransactionFacade = agentTransactionFacade;
        _clientProps = clientProps;
        _logger = logger;
    }

    public async Task<CancelTransactionResult> ExecuteAsync(CancelTransactionDto request)
    {
        _logger.LogInformation(
            "🛑 Starting cancellation workflow for transaction {TxnRef}",
            request.TawakalTxnRef);

        // 1️⃣ Validate transaction status
        var status = await _agentTransactionFacade
            .ValidateTransactionStatusAsync(request.TawakalTxnRef);

        if (string.IsNullOrWhiteSpace(status))
        {
            throw new TransactionNotFoundException(request.TawakalTxnRef);
        }

        _logger.LogInformation(
            "✅ Transaction {TxnRef} current status: {Status}",
            request.TawakalTxnRef,
            status);

        // 2️⃣ Check cancellable rule
        if (!IsCancellable(status))
        {
            return new CancelTransactionResult(
                Success: false,
                Status: "NOT_CANCELLABLE",
                Message: "Transaction is not valid for cancellation",
                TransactionReference: request.TawakalTxnRef
            );
        }

        // 3️⃣ Build provider request
        var providerRequest = BuildCancelRequest(request);

        // 4️⃣ Call cancellation service
        var providerResult =
            await _cancellationService.CancelTransactionAsync(providerRequest);

        if (providerResult is null)
        {
            return new CancelTransactionResult(
                Success: false,
                Status: "FAILED",
                Message: "Cancellation failed — provider did not respond",
                TransactionReference: request.TawakalTxnRef
            );
        }

        // 5️⃣ Success
        return new CancelTransactionResult(
            Success: true,
            Status: "Cancelled",
            Message: "Transaction successfully cancelled",
            TransactionReference: request.TawakalTxnRef
        );
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static bool IsCancellable(string status)
        => "READY".Equals(status, StringComparison.OrdinalIgnoreCase);

    private CancelReceiveRequest BuildCancelRequest(CancelTransactionDto dto)
    {
        return new CancelReceiveRequest
        {
            Refno = dto.TawakalTxnRef,
            Reason = dto.Reason,
            Loccode = _clientProps.Loccode,
            Agtaprvduser = _clientProps.UserCred,
            Rqstdate = DateTime.UtcNow,
            Action = "CANCEL"
        };
    }
}

/// <summary>
/// Result returned by the CancelTransactionOrchestrator.
/// This is NOT an HTTP response.
/// </summary>
public sealed record CancelTransactionResult(
    bool Success,
    string Status,
    string Message,
    string TransactionReference
);