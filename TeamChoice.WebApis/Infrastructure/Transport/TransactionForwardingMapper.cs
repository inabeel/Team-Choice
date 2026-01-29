using TeamChoice.WebApis.Domain.Models.Transactions;

namespace TeamChoice.WebApis.Infrastructure.Transport;

public sealed class TransactionForwardingMapper
{
    public ForwardingRequestDto Map(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        return new ForwardingRequestDto(
            Reference: transaction.Reference,
            Amount: transaction.Amount,
            Currency: transaction.Currency,
            ServiceType: transaction.ServiceType);
    }
}