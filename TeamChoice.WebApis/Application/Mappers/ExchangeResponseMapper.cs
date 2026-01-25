using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Models;

namespace TeamChoice.WebApis.Application.Mappers;

public static class ExchangeResponseMapper
{
    /// <summary>
    /// Maps internal domain exchange response to API response
    /// </summary>
    public static ExchangeResponseBuilderDto ToApiResponse(
        ExchangeRateResponseDto domainResponse)
    {
        ArgumentNullException.ThrowIfNull(domainResponse.ExchangeDetails);
        ArgumentNullException.ThrowIfNull(domainResponse.ExchangeDetails.Payer);
        ArgumentNullException.ThrowIfNull(domainResponse.ExchangeDetails.Recipient);

        return new ExchangeResponseBuilderDto
        {
            Timestamp = DateTime.UtcNow,
            StatusCode = 200,
            StatusMessage = "OK",

            ExchangeDetails = new ExchangeDetailsDto
            {
                Payer = new PayerDto
                {
                    AmountDue = domainResponse.ExchangeDetails.Payer.AmountDue,
                    ExchangeRate = domainResponse.ExchangeDetails.Payer.ExchangeRate,
                    TransactionFee = domainResponse.ExchangeDetails.Payer.TransactionFee,
                    CurrencyCode = "USD" // enrichment belongs here
                },
                Recipient = new RecipientDto
                {
                    Amount = domainResponse.ExchangeDetails.Recipient.Amount,
                    CurrencyCode = "ETB"
                }
            }
        };
    }

    /// <summary>
    /// Maps raw exchange rate result into internal domain response
    /// (NO calculations here)
    /// </summary>
    public static InternalExchangeRateResult MapToExchangeRateResponse(ExchangeRateResult rate)
    {
        return new InternalExchangeRateResult
        {
            ExchangeDetails = new ExchangeDetailsDto
            {
                Payer = new PayerDto
                {
                    AmountDue = rate.Usd,
                    ExchangeRate = 0m,        // calculated later
                    TransactionFee = 0m      // calculated later
                },
                Recipient = new RecipientDto
                {
                    Amount = rate.EtbIr
                }
            }
        };
    }
}
