namespace TeamChoice.WebApis.Infrastructure.Transport;

public sealed class CallbackPayload
{
    public string TransactionReference { get; init; } = default!;
    public string PartnerReference { get; init; } = default!;
    public string Status { get; init; } = default!;
    public string? ProviderReference { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}