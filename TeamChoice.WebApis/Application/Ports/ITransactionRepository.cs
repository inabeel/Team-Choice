using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Models.Transactions;

namespace TeamChoice.WebApis.Application.Ports;

/// <summary>
/// Persistence boundary for transaction aggregates.
/// Implemented using legacy SQL / stored procedures.
/// Called ONLY by TransactionWorkflowService.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Checks whether a transaction already exists for the given partner reference.
    /// Java equivalent: agentTransactionFacade.validateTransaction(...)
    /// </summary>
    Task<bool> ExistsByPartnerReferenceAsync(
        string partnerReference,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists a newly created transaction.
    /// Must be called before forwarding.
    /// </summary>
    Task SaveAsync(
        Transaction transaction,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a transaction by its internal reference.
    /// Returns null if not found.
    /// </summary>
    Task<Transaction?> GetByReferenceAsync(
        string transactionReference,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing transaction (status changes, cancellation, etc.).
    /// No-op if transaction does not exist.
    /// </summary>
    Task UpdateAsync(
        Transaction transaction,
        CancellationToken cancellationToken);

    Task<string> ValidateTransactionStatusAsync(
    string transactionCode,
    CancellationToken cancellationToken);

    Task CancelAsync(CancelReceiveRequest cancelRequest, CancellationToken cancellationToken);
}
