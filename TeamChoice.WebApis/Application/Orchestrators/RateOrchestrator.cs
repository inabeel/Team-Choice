using TeamChoice.WebApis.Application.Facades;
using TeamChoice.WebApis.Application.Interfaces.Repositories;
using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Domain.Constants;
using TeamChoice.WebApis.Domain.Models.DTOs;
using TeamChoice.WebApis.Domain.Models.DTOs.Exchanges;

namespace TeamChoice.WebApis.Application.Orchestrators;

public interface IRateOrchestrator
{
    Task<ExchangeRateResponseDto> GetInternalRateAsync(ExchangeRatePayloadDto payload);

    Task<ExchangeResponseBuilderDto> GetExternalRateAsync(ExchangeRatePayloadDto payload);
}

public sealed class RateOrchestrator : IRateOrchestrator
{
    private readonly IAgentTransactionFacade _agentFacade;
    private readonly IRateRepository _rateRepository;

    public RateOrchestrator(IAgentTransactionFacade agentFacade, IRateRepository rateRepository)
    {
        _agentFacade = agentFacade;
        _rateRepository = rateRepository;
    }

    public async Task<ExchangeRateResponseDto> GetInternalRateAsync(ExchangeRatePayloadDto payload)
    {
        var query = new ExchangeRateQueryDto
        {
            AgentCode = RateConstants.DEFAULT_AGENT_CODE,
            LocationCode = RateConstants.DEFAULT_LOCATION_CODE,
            CurrencyCode = RateConstants.DEFAULT_CURRENCY_CODE
        };

        var response = await _agentFacade.GetExchangeRateAsync(query);

        return RateCalculator.ApplyInternalRate(response, payload);
    }

    public async Task<ExchangeResponseBuilderDto> GetExternalRateAsync(ExchangeRatePayloadDto payload)
    {
        var commission = await _rateRepository.CalculateExternalPartnerCommissionAsync(payload);

        return RateCalculator.BuildExternalResponse(commission, payload);
    }
}

public static class RateCalculator
{
    public static ExchangeRateResponseDto ApplyInternalRate(ExchangeRateResponseDto rateResponse, ExchangeRatePayloadDto payload)
    {
        var payer = rateResponse.ExchangeDetails!.Payer!;
        var recipient = rateResponse.ExchangeDetails!.Recipient!;

        var exchangeRate = Math.Round(payer.AmountDue, 4);

        var amountDue = Math.Round(exchangeRate * payload.RecipientAmount, 2);

        var transactionFee = Math.Round(amountDue * RateConstants.TRANSACTION_FEE_PERCENT, 4);

        return rateResponse with
        {
            ExchangeDetails = rateResponse.ExchangeDetails with
            {
                Payer = payer with
                {
                    ExchangeRate = exchangeRate,
                    AmountDue = amountDue,
                    TransactionFee = transactionFee
                },
                Recipient = recipient with
                {
                    Amount = payload.RecipientAmount
                }
            }
        };
    }

    public static ExchangeResponseBuilderDto BuildExternalResponse(CommissionResultDTO rateResponse, ExchangeRatePayloadDto payload)
    {
        return new ExchangeResponseBuilderDto
        {
            Timestamp = DateTime.UtcNow,
            StatusMessage = "OK",
            StatusCode = StatusCodes.Status200OK,
            ExchangeDetails = new ExchangeResponseBuilderDto.ExchangeDetailsDto
            {
                Payer = new ExchangeResponseBuilderDto.PayerDto
                {
                    AmountDue = payload.SendingAmount,
                    CurrencyCode = rateResponse.SendingCurrencyCode,
                    ExchangeRate = rateResponse.ExchangeRate,
                    TransactionFee = rateResponse.TransactionFee
                },
                Recipient = new ExchangeResponseBuilderDto.RecipientDto
                {
                    Amount = rateResponse.Amount,
                    CurrencyCode = rateResponse.ReceivingCurrencyCode
                }
            }
        };
    }

}
