using TeamChoice.WebApis.Domain.Models.DTOs.Exchanges;

namespace TeamChoice.WebApis.Application.Interfaces.Services;

public interface IAgentTransactionFacade
{
    Task<SMTMstMpUser> GetUserByLocCodeAsync(string locCode);

    Task<string> GetRecLocCodeAsync(string country, string serviceCode);

    Task<decimal> FindRcvcomByAgtCodeAsync(string agtCode);

    Task<OutboundProviderCredential> FindOutboundCredAsync(string serviceCode);

    Task<decimal> FindSmtCommissionAsync(string trnsCode);

    Task<string> FindTrnsCodeFromSmtTransactionsAsync(string referenceNumber);

    Task<string> FindServiceCodeUsingBankServiceTypeAsync(string serviceCode);

    Task<string> ValidateTransactionAsync(string partnerReference);

    Task<string> ValidateTransactionStatusAsync(string authNumber);

    Task<long> InsertPartnerTransactionAsync(PartnerTransaction partnerTransaction);

    Task<long> UpdateAfterPayingTransactionAsync(string trnsCode, string newStatus);

    Task<ExchangeRateResponseDto> GetExchangeRateAsync(ExchangeRateQueryDto exchangeRateQuery);

    Task<int> GetLocIdForCashPickupServiceAsync(string serviceCode);
}
