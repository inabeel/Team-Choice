using TeamChoice.WebApis.Domain.Models.Transactions;

namespace TeamChoice.WebApis.Application.Ports;

public interface ITransactionForwarder
{
    /// <summary>
    /// Forwards a transaction to the downstream remittance system.
    /// </summary>
    /// <exception cref="TransactionForwardingException">
    /// Thrown when forwarding fails.
    /// </exception>
    Task<ForwardingResult> ForwardAsync(Transaction transaction, CancellationToken cancellationToken);
}