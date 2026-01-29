namespace TeamChoice.WebApis.Infrastructure.Transport;

public sealed record TransactionCallbackRequestDto(
    string TransactionReference,
    string ExternalTransactionId,
    string Status,
    DateTimeOffset Timestamp
);