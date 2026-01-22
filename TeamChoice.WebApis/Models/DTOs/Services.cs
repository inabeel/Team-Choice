namespace TeamChoice.WebApis.Models.DTOs.Services;

public record AccountLookupRequestDto(
    string ServiceCode,
    string ServiceMode
);

public record AccountsLookupRequestDto(
    string ServiceCode,
    string PhoneNumber,
    string? PaymentMode
);

public record AccountLookupResponseDto
{
    public string? Response { get; init; }
    public string? ServiceType { get; init; }
    public string? ServiceCode { get; init; }
    public string? ServiceName { get; init; }
    public string? ServiceMode { get; init; }
    public string? AccountName { get; init; }
    public string? CountryCode { get; init; }
    public string? CurrencyCode { get; init; }
}

public record ServiceDetailDto
{
    public string? ProviderName { get; init; }
    public string? ServiceType { get; init; }
    public string? ServiceName { get; init; }
    public string? ServiceCode { get; init; }

    public decimal? MinAmount { get; init; }
    public decimal? MaxAmount { get; init; }

    public string? CurrencyCode { get; init; }
    public string? CountryCode { get; init; }

    public string? Description { get; init; }
    public bool Active { get; init; }
}

public record ServiceLookupResponseDto
{
    /// <summary>
    /// Indicates whether the lookup was successful.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Optional response message.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// List of services returned by the lookup.
    /// </summary>
    public IReadOnlyCollection<ServiceDetailDto> Services { get; init; }
        = Array.Empty<ServiceDetailDto>();
}