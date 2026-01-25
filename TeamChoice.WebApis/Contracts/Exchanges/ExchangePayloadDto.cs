using System.ComponentModel.DataAnnotations;

namespace TeamChoice.WebApis.Contracts.Exchanges;

/// <summary>
/// Payload to compute external partner exchange rate and fees.
/// </summary>
public sealed class ExchangePayloadDto
{
    /// <summary>
    /// Sending country (ISO 3166-1 alpha-2).
    /// </summary>
    /// <example>SO</example>
    [Required]
    public string SendingCountry { get; init; } = default!;

    /// <summary>
    /// Sending currency (ISO 4217).
    /// </summary>
    /// <example>USD</example>
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string CurrencyCode { get; init; } = default!;

    /// <summary>
    /// Service code.
    /// </summary>
    /// <example>231404</example>
    [Required]
    public string ServiceCode { get; init; } = default!;

    /// <summary>
    /// Recipient country (ISO 3166-1 alpha-2).
    /// </summary>
    /// <example>ET</example>
    [Required]
    public string RecipientCountry { get; init; } = default!;

    /// <summary>
    /// Amount that the customer pays.
    /// </summary>
    /// <example>100.00</example>
    [Required]
    public decimal SendingAmount { get; init; }
}

/// <summary>
/// Payload to request internal exchange rate.
/// </summary>
public sealed class ExchangeRatePayloadDto
{
    /// <summary>
    /// Location code.
    /// </summary>
    /// <example>LOC-001</example>
    public string? LocationCode { get; init; }

    /// <summary>
    /// Service code.
    /// </summary>
    /// <example>231404</example>
    [Required]
    public string ServiceCode { get; init; } = default!;

    /// <summary>
    /// Recipient amount in target currency.
    /// </summary>
    /// <example>150.00</example>
    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal RecipientAmount { get; init; }

    /// <summary>
    /// Recipient currency code (ISO 4217).
    /// </summary>
    /// <example>ETB</example>
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string RecipientCurrencyCode { get; init; } = default!;

    /// <summary>
    /// Country code (ISO 3166-1 alpha-2).
    /// </summary>
    /// <example>ET</example>
    [Required]
    [RegularExpression("^[A-Z]{2}$", ErrorMessage = "Country code must be a valid ISO 3166-1 alpha-2 code")]
    public string CountryCode { get; init; } = default!;
}
