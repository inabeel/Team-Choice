using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Domain.Models.Transactions;

namespace TeamChoice.WebApis.Application.Ports;

public interface ITransactionCallbackClient
{
    /// <summary>
    /// Notifies downstream systems about the transaction result.
    /// </summary>
    /// <remarks>
    /// Callback failures are considered non-fatal unless explicitly required
    /// by business rules.
    /// </remarks>
    Task NotifyAsync(Transaction transaction, ForwardingResult forwardingResult, CancellationToken cancellationToken);
}
