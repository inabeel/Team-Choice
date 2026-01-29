namespace TeamChoice.WebApis.Domain.Configuration;

public sealed class RateLimitSettings
{
    public int PermitLimit { get; set; } = 100;
    public double WindowMinutes { get; set; } = 1;
    public int QueueLimit { get; set; } = 2;
}
