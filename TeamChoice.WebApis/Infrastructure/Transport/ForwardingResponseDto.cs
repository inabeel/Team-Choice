namespace TeamChoice.WebApis.Infrastructure.Transport;

public sealed record ForwardingResponseDto(string TransactionId, DateTimeOffset Timestamp, string Status);