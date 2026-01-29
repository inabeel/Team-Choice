namespace TeamChoice.WebApis.Domain.Models.Transactions;

public sealed record ForwardingResult(string ExternalTransactionId, DateTimeOffset ForwardedAt, string Status);