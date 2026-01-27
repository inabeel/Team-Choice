using TeamChoice.WebApis.Application.Interfaces.Repositories;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Infrastructure.Persistence;
using TeamChoice.WebApis.Utils;

namespace TeamChoice.WebApis.Infrastructure.Repositories;

public class RateRepository : IRateRepository
{
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<RateRepository> _logger;

    public RateRepository(IDatabaseService databaseService, ILogger<RateRepository> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task<CommissionResultDTO> CalculateExternalPartnerCommissionAsync(ExchangePayloadDto exchangePayload)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@SendingCountry", exchangePayload.SendingCountry },
            { "@CurrencyCode", exchangePayload.CurrencyCode },
            { "@ServiceCode", exchangePayload.ServiceCode },
            { "@RecipientCountry", exchangePayload.RecipientCountry },
            { "@SendingAmount", exchangePayload.SendingAmount }
        };

        try
        {
            // Execute stored procedure
            return await _databaseService.ExecuteStoredProcedureAsync(
                RateSqlQueries.CALCULATE_EXTERNAL_PARTNER_COMMISSION,
                parameters,
                reader => new CommissionResultDTO
                {
                    Amount = Convert.ToDecimal(reader["Amount"]),
                    ReceivingCurrencyCode = reader["ReceivingCurrencyCode"] as string,
                    AmountDue = Convert.ToDecimal(reader["AmountDue"]),
                    SendingCurrencyCode = reader["SendingCurrencyCode"] as string,
                    ExchangeRate = Convert.ToDecimal(reader["ExchangeRate"]),
                    TransactionFee = Convert.ToDecimal(reader["TransactionFee"])
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error while executing commission stored procedure for payload: {@Payload}", exchangePayload);
            throw; // Re-throw to be handled by controller/middleware
        }
    }
}
