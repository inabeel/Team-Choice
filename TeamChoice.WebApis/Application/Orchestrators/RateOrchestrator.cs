using TeamChoice.WebApis.Application.Interfaces.Repositories;
using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Constants;
using TeamChoice.WebApis.Domain.Models;

namespace TeamChoice.WebApis.Application.Orchestrators;

public interface IRateOrchestrator
{
    Task<InternalExchangeRateResult> GetInternalRateAsync(ExchangeRatePayloadDto payload);

    Task<ExchangeResponseBuilderDto> GetExternalRateAsync(ExchangePayloadDto payload);
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

    public async Task<InternalExchangeRateResult> GetInternalRateAsync(ExchangeRatePayloadDto payload)
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

    public async Task<ExchangeResponseBuilderDto> GetExternalRateAsync(ExchangePayloadDto payload)
    {
        var commission =
            await _rateRepository.CalculateExternalPartnerCommissionAsync(payload);

        return RateCalculator.BuildExternalResponse(commission, payload);
    }
}

public static class RateCalculator
{
    public static InternalExchangeRateResult ApplyInternalRate(InternalExchangeRateResult rateResponse, ExchangeRatePayloadDto payload)
    {
        ArgumentNullException.ThrowIfNull(rateResponse);
        ArgumentNullException.ThrowIfNull(payload);

        var exchangeDetails = rateResponse.ExchangeDetails
            ?? throw new InvalidOperationException("ExchangeDetails is missing");

        var payer = exchangeDetails.Payer
            ?? throw new InvalidOperationException("Payer is missing");

        var recipient = exchangeDetails.Recipient
            ?? throw new InvalidOperationException("Recipient is missing");

        // Java: payer.getAmountDue().setScale(4, HALF_UP)
        var exchangeRate = Math.Round(payer.AmountDue, 4, MidpointRounding.AwayFromZero);

        // Java: exchangeRate.multiply(recipientAmount).setScale(2, HALF_UP)
        var amountDue = Math.Round(exchangeRate * payload.RecipientAmount, 2, MidpointRounding.AwayFromZero);

        // Java: amountDue.multiply(FEE_PERCENT).setScale(4, HALF_UP)
        var transactionFee = Math.Round(amountDue * RateConstants.TRANSACTION_FEE_PERCENT, 4, MidpointRounding.AwayFromZero);

        return rateResponse with
        {
            ExchangeDetails = exchangeDetails with
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

    public static ExchangeResponseBuilderDto BuildExternalResponse(CommissionResultDTO commission, ExchangePayloadDto payload)
    {
        ArgumentNullException.ThrowIfNull(commission);
        ArgumentNullException.ThrowIfNull(payload);

        return new ExchangeResponseBuilderDto
        {
            Timestamp = DateTime.UtcNow,
            StatusMessage = "OK",
            StatusCode = 200,

            ExchangeDetails = new ExchangeDetailsDto
            {
                Payer = new PayerDto
                {
                    AmountDue = payload.SendingAmount,
                    CurrencyCode = commission.SendingCurrencyCode,
                    ExchangeRate = commission.ExchangeRate,
                    TransactionFee = commission.TransactionFee
                },
                Recipient = new RecipientDto
                {
                    Amount = commission.Amount,
                    CurrencyCode = commission.ReceivingCurrencyCode
                }
            }
        };
    }
}