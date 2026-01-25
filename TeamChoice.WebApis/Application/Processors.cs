using TeamChoice.WebApis.Application.Orchestrators;
using TeamChoice.WebApis.Domain.Models.DTOs;
using TeamChoice.WebApis.Domain.Models.DTOs.Exchanges;

namespace TeamChoice.WebApis.Application;

public interface ITransactionProcessor
{
    Task<TransactionOrchestrationResult> ProcessAsync(
        TransactionRequestDto request);

    PartnerTransaction BuildPartnerTransaction(TransactionRequestDto request);
}

/// <summary>
/// Builds a PartnerTransaction from a transaction request.
/// </summary>
public sealed class TransactionProcessor : ITransactionProcessor
{
    private const string StatusPending = "PENDING";

    public PartnerTransaction BuildPartnerTransaction(TransactionRequestDto request)
    {
        // NOTE: Preserving Java behavior exactly
        var transactionDate = "2025-10-17T02:44:00Z";

        return new PartnerTransaction
        {
            TransactionDate = transactionDate,
            PartnerReference = request.PartnerReference,
            PartnerCode = request.SendingLocation?.LocationCode,
            Status = StatusPending,
            Payload = request.ToString()
        };
    }

    public Task<TransactionOrchestrationResult> ProcessAsync(TransactionRequestDto request)
    {
        throw new NotImplementedException();
    }
}