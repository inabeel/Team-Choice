using TeamChoice.WebApis.Domain.Models.Enums;

namespace TeamChoice.WebApis.Domain.Models.Transactions;

public sealed record TransactionResult(string Reference, TransactionStatus Status)
{
    public static TransactionResult Pending(string reference)
        => new(reference, TransactionStatus.Pending);

    public static TransactionResult Cancelled(string reference)
        => new(reference, TransactionStatus.Cancelled);

    public static TransactionResult From(Transaction transaction)
        => new(transaction.Reference, transaction.Status);
}