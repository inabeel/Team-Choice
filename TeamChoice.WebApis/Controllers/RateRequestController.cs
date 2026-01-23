using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Infrastructure.Repositories;
using TeamChoice.WebApis.Models.DTOs;

namespace TeamChoice.WebApis.Controllers
{
    [ApiController]
    [Route("api/v1/rate")]
    public class RateRequestController : ControllerBase
    {
        private readonly IAgentTransactionFacade _agentTransactionFacade;
        private readonly IRateRepository _rateRepository;
        private readonly ILogger<RateRequestController> _logger;

        public RateRequestController(
            IAgentTransactionFacade agentTransactionFacade,
            IRateRepository rateRepository,
            ILogger<RateRequestController> logger)
        {
            _agentTransactionFacade = agentTransactionFacade;
            _rateRepository = rateRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetExchangeRate([FromBody] ExchangePayload payload)
        {
            _logger.LogInformation("📥 Received exchange rate request: {@Payload}", payload);

            try
            {
                var query = BuildQueryFromPayload(payload);
                var rateResponse = await _agentTransactionFacade.GetExchangeRateAsync(query);
                var transformedResponse = TransformRateResponse(rateResponse, payload);

                _logger.LogInformation("✅ Exchange rate processed successfully");
                return Ok(transformedResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "❌ Failed to process exchange rate");
                // In a real app, you might return 500 or 400 depending on the exception
                return StatusCode(500);
            }
        }

        [HttpPost("exchange-rate")]
        public async Task<IActionResult> GetRate([FromBody] ExchangePayload payload)
        {
            _logger.LogInformation("📥 Received external partner exchange rate request: {@Payload}", payload);

            try
            {
                var rateResponse = await _rateRepository.CalculateExternalPartnerCommissionAsync(payload);
                var response = BuildExchangeResponse(rateResponse, payload);

                _logger.LogInformation("✅ Commission rate processed successfully");
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "❌ Failed to process commission rate");
                return StatusCode(500);
            }
        }

        private ExchangeRateQuery BuildQueryFromPayload(ExchangePayload payload)
        {
            return new ExchangeRateQuery
            {
                AgtCode = RateConstants.DEFAULT_AGENT_CODE,
                LocCode = RateConstants.DEFAULT_LOCATION_CODE,
                CurCode = RateConstants.DEFAULT_CURRENCY_CODE
            };
        }

        private ExchangeRateResponse TransformRateResponse(ExchangeRateResponse rateResponse, ExchangePayload payload)
        {
            var payer = rateResponse.ExchangeDetails.Payer;
            var recipient = rateResponse.ExchangeDetails.Recipient;

            decimal recipientAmount = payload.RecipientAmount;
            recipient.Amount = recipientAmount;

            // Math.Round with MidpointRounding.AwayFromZero is roughly equivalent to Java's RoundingMode.HALF_UP
            decimal exchangeRate = Math.Round(payer.AmountDue, 4, MidpointRounding.AwayFromZero);
            payer.ExchangeRate = exchangeRate;

            decimal amountDue = Math.Round(exchangeRate * recipientAmount, 2, MidpointRounding.AwayFromZero);
            payer.AmountDue = amountDue;

            decimal fee = Math.Round(amountDue * RateConstants.TRANSACTION_FEE_PERCENT, 4, MidpointRounding.AwayFromZero);
            payer.TransactionFee = fee;

            return rateResponse;
        }

        private ExchangeRateResponse BuildExchangeResponse(CommissionResultDTO rateResponse, ExchangePayload payload)
        {
            return new ExchangeRateResponse
            {
                Timestamp = DateTime.Now,
                StatusMessage = "OK",
                StatusCode = 200,
                ExchangeDetails = new ExchangeDetails
                {
                    Payer = new Payer
                    {
                        AmountDue = payload.SendingAmount,
                        CurrencyCode = rateResponse.SendingCurrencyCode,
                        ExchangeRate = rateResponse.ExchangeRate,
                        TransactionFee = rateResponse.TransactionFee
                    },
                    Recipient = new Recipient
                    {
                        Amount = rateResponse.Amount,
                        CurrencyCode = rateResponse.ReceivingCurrencyCode
                    }
                }
            };
        }
       
    }
    public class RateConstants
    {
        public const string DEFAULT_AGENT_CODE = "AGT001";
        public const string DEFAULT_LOCATION_CODE = "LOC001";
        public const string DEFAULT_CURRENCY_CODE = "USD";
        public const decimal TRANSACTION_FEE_PERCENT = 0.02M; // 2% fee
    }
}
