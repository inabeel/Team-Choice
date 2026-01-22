namespace TeamChoice.WebApis.Domain.Models;

/// <summary>
/// Response model for service lookup operations.
/// </summary>
public sealed class ServiceLookupResponse
{
    public string? Response { get; set; }
    public string? ServiceCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AccountName { get; set; }
    public string? AccountId { get; set; }
    public string? Provider { get; set; }
    public string? CountryCode { get; set; }
    public string? CurrencyCode { get; set; }
}