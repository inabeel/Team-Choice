using TeamChoice.WebApis.Domain.Models.DTOs.Exchanges;

namespace TeamChoice.WebApis.Application.Interfaces.Repositories;

public interface IAgentRepository
{
    Task<SMTMstMpUser> GetUserByLocCodeAsync(string locCode);
    Task<string> GetRecLocCodeAsync(string country);
    Task<decimal> FindRcvcomByAgtCodeAsync(string agtCode);
    Task<OutboundProviderCredential> FindOutboundCredAsync(string serviceCode);
    Task<decimal> FindSmtCommissionAsync(string trnsCode);
    Task<string> FindTrnsCodeFromSmtTransactionsAsync(string referenceNumber);
    Task<int?> FindServiceCodeUsingBankServiceTypeAsync(string serviceCode);
    Task<string> ValidateTransactionAsync(string partnerReference);
    Task<string> ValidateTransactionStatusAsync(string authNumber);
    Task<long> InsertPartnerTransactionAsync(PartnerTransaction partnerTransaction);
    Task<long> UpdateAfterPayingTransactionAsync(string trnsCode, string newStatus);
    Task<ExchangeRateResult> GetExchangeRateAsync(ExchangeRateQueryDto query);
    Task<int?> GetLocIdForServiceCodeAsync(string serviceCode);
}