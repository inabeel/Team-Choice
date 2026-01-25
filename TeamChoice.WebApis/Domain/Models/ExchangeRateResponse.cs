using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Domain.Models;

/// <summary>
/// Internal exchange rate response (used by orchestrators/services)
/// </summary>
public record InternalExchangeRateResult
{
    public ExchangeDetailsDto? ExchangeDetails { get; init; }
}