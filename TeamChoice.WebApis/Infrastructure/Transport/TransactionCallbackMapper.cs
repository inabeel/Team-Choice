using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Domain.Models.Transactions;

namespace TeamChoice.WebApis.Infrastructure.Transport;

public sealed class TransactionCallbackMapper
{
    public TransactionCallbackRequestDto Map(Transaction transaction, ForwardingResult forwardingResult)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        ArgumentNullException.ThrowIfNull(forwardingResult);

        return new TransactionCallbackRequestDto(
            TransactionReference: transaction.Reference,
            ExternalTransactionId: forwardingResult.ExternalTransactionId,
            Status: forwardingResult.Status,
            Timestamp: forwardingResult.ForwardedAt
        );
    }
}
