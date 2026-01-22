namespace TeamChoice.WebApis.Domain.Models;

public class ServiceDetail
{
    public string? ProviderName { get; set; }
    public string? ServiceType { get; set; }
    public string? ServiceName { get; set; }
    public string? ServiceCode { get; set; }
    public string? MinAmount { get; set; }
    public string? MaxAmount { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CountryCode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}