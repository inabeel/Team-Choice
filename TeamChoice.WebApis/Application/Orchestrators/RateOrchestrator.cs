using TeamChoice.WebApis.Application.Facades;
using TeamChoice.WebApis.Domain.Constants;
using TeamChoice.WebApis.Domain.Models.DTOs;
using TeamChoice.WebApis.Infrastructure.Repositories;

namespace TeamChoice.WebApis.Application.Orchestrators;

public interface IRateOrchestrator
{
    Task<ExchangeRateResponseDto> GetInternalRateAsync(ExchangeRatePayloadDto payload);
    Task<ExchangeResponseBuilderDto> GetExternalRateAsync(ExchangePayloadDto payload);
}

public sealed class RateOrchestrator : IRateOrchestrator
{
    private readonly IAgentTransactionFacade _agentFacade;
    private readonly IRateRepository _rateRepository;

    public RateOrchestrator(
        IAgentTransactionFacade agentFacade,
        IRateRepository rateRepository)
    {
        _agentFacade = agentFacade;
        _rateRepository = rateRepository;
    }

    public async Task<ExchangeRateResponseDto> GetInternalRateAsync(
        ExchangeRatePayloadDto payload)
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

    public async Task<ExchangeResponseBuilderDto> GetExternalRateAsync(
        ExchangePayloadDto payload)
    {
        var commission = await _rateRepository.CalculateExternalPartnerCommissionAsync(payload);

        return RateCalculator.BuildExternalResponse(commission, payload);
    }
}

public static class RateCalculator
{
    public static ExchangeRateResponseDto ApplyInternalRate(
        ExchangeRateResponseDto rateResponse,
        ExchangeRatePayloadDto payload)
    {
        var payer = rateResponse.ExchangeDetails.Payer;
        var recipient = rateResponse.ExchangeDetails.Recipient;

        // Set recipient amount from request
        recipient.Amount = (decimal)payload.RecipientAmount;

        // Exchange rate with precision
        payer.ExchangeRate = Math.Round(payer.AmountDue, 4);

        // Amount due = rate × recipient amount
        payer.AmountDue = Math.Round(
            payer.ExchangeRate * (decimal)payload.RecipientAmount,
            2);

        // Transaction fee
        payer.TransactionFee = Math.Round(
            payer.AmountDue * RateConstants.TRANSACTION_FEE_PERCENT,
            4);

        return rateResponse;
    }


    public static ExchangeResponseBuilderDto BuildExternalResponse(
    CommissionResultDTO rateResponse,
    ExchangePayloadDto payload)
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
