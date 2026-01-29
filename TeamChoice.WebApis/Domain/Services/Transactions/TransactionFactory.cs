

using TeamChoice.WebApis.Application.Commands.Transactions;
using TeamChoice.WebApis.Domain.Models.Transactions;

namespace TeamChoice.WebApis.Domain.Services.Transactions;

public sealed class TransactionFactory
{
    public Transaction Create(CreateTransactionCommand command, string resolvedServiceType)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reference = GenerateReference();

        return Transaction.Create(
            reference: reference,
            partnerReference: command.PartnerReference,
            amount: command.Amount,
            currency: command.Currency,
            serviceType: resolvedServiceType);
    }

    private static string GenerateReference()
        => Guid.NewGuid().ToString("N");
}
