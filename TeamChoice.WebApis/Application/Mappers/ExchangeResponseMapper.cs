using TeamChoice.WebApis.Domain.Models.DTOs.Exchanges;

namespace TeamChoice.WebApis.Application.Mappers;

public static class ExchangeResponseMapper
{
    public static ExchangeResponseBuilderDto ToApiResponse(ExchangeRateResponseDto domainResponse)
    {
        return new ExchangeResponseBuilderDto
        {
            Timestamp = DateTime.UtcNow,
            StatusCode = 200,
            StatusMessage = "OK",

            ExchangeDetails = new ExchangeResponseBuilderDto.ExchangeDetailsDto
            {
                Payer = new ExchangeResponseBuilderDto.PayerDto
                {
                    AmountDue = domainResponse.ExchangeDetails!.Payer!.AmountDue,
                    ExchangeRate = domainResponse.ExchangeDetails.Payer.ExchangeRate,
                    TransactionFee = domainResponse.ExchangeDetails.Payer.TransactionFee,
                    CurrencyCode = "USD" // enrich here
                },
                Recipient = new ExchangeResponseBuilderDto.RecipientDto
                {
                    Amount = domainResponse.ExchangeDetails.Recipient!.Amount,
                    CurrencyCode = "ETB"
                }
            }
        };
    }

    public static ExchangeRateResponseDto MapToExchangeRateResponseDto(ExchangeRateResult rate)
    {
        throw new NotImplementedException();
    }
}
