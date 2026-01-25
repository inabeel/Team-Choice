using System.ComponentModel.DataAnnotations;

namespace TeamChoice.WebApis.Contracts.Exchanges;

#region Common Exchange DTOs (SHARED)

public record ExchangeDetailsDto
{
    public PayerDto? Payer { get; init; }
    public RecipientDto? Recipient { get; init; }
}

public record PayerDto
{
    public decimal AmountDue { get; init; }
    public decimal ExchangeRate { get; init; }
    public decimal TransactionFee { get; init; }
    public string? CurrencyCode { get; init; }
}

public record RecipientDto
{
    public decimal Amount { get; init; }
    public string? CurrencyCode { get; init; }
}

#endregion

#region Responses

/// <summary>
/// Response containing computed internal exchange rate details.
/// </summary>
public sealed class ExchangeRateResponseDto
{
    /// <summary>
    /// Response timestamp (ISO 8601).
    /// </summary>
    /// <example>2025-10-17T02:45:00Z</example>
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// Status code.
    /// </summary>
    /// <example>200</example>
    public int StatusCode { get; init; }

    /// <summary>
    /// Status message.
    /// </summary>
    /// <example>OK</example>
    public string StatusMessage { get; init; } = default!;

    /// <summary>
    /// Exchange details.
    /// </summary>
    public ExchangeDetailsDto ExchangeDetails { get; init; } = default!;
}



/// <summary>
/// External / API response wrapper
/// </summary>
public record ExchangeResponseBuilderDto
{
    public DateTime Timestamp { get; init; }
    public string? StatusMessage { get; init; }
    public int StatusCode { get; init; }
    public ExchangeDetailsDto? ExchangeDetails { get; init; }
}

#endregion
