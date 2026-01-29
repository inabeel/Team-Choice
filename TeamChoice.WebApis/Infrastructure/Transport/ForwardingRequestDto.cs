namespace TeamChoice.WebApis.Infrastructure.Transport;

public sealed record ForwardingRequestDto(
    string Reference,
    decimal Amount,
    string Currency,
    string ServiceType
);