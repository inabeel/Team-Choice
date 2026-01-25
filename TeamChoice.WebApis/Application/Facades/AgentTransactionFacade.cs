using TeamChoice.WebApis.Application.Interfaces.Repositories;
using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Application.Mappers;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Models;

namespace TeamChoice.WebApis.Application.Facades;

public class AgentTransactionFacade : IAgentTransactionFacade
{
    private readonly IAgentRepository _agentRepository;
    private readonly ILogger<AgentTransactionFacade> _logger;
    private readonly IConfiguration _configuration;

    private readonly string _somBankLocation;
    private readonly string _tPlusLocation;
    private readonly string _mmtSoLocation;
    private readonly string _mmtKeLocation;

    public AgentTransactionFacade(IAgentRepository agentRepository, ILogger<AgentTransactionFacade> logger, IConfiguration configuration)
    {
        _agentRepository = agentRepository;
        _logger = logger;
        _configuration = configuration;

        _somBankLocation = _configuration["somBankLocation"];
        _tPlusLocation = _configuration["tPlusLocation"];
        _mmtSoLocation = _configuration["mmtSoLocation"];
        _mmtKeLocation = _configuration["mmtKeLocation"];
    }

    public async Task<SMTMstMpUser> GetUserByLocCodeAsync(string locCode)
    {
        _logger.LogDebug("🔍 Validating location code: {LocCode}", locCode);
        return await _agentRepository.GetUserByLocCodeAsync(locCode);
    }

    public async Task<string> GetRecLocCodeAsync(string country, string serviceCode)
    {
        return serviceCode switch
        {
            "00003" => _tPlusLocation,
            "00010" => _somBankLocation,
            "00006" => _mmtSoLocation,
            "00014" => _mmtKeLocation,
            _ => await _agentRepository.GetRecLocCodeAsync(country) ??
                 throw new ArgumentException($"Location code not found for country={country}, and serviceCode={serviceCode}")
        };
    }

    public async Task<decimal> FindRcvcomByAgtCodeAsync(string agtCode)
    {
        _logger.LogDebug("💰 Fetching receive commission for agent code: {AgtCode}", agtCode);
        return await _agentRepository.FindRcvcomByAgtCodeAsync(agtCode);
    }

    public async Task<OutboundProviderCredential> FindOutboundCredAsync(string serviceCode)
    {
        _logger.LogDebug("🔐 Fetching outbound credentials for serviceCode: {ServiceCode}", serviceCode);
        return await _agentRepository.FindOutboundCredAsync(serviceCode);
    }

    public async Task<decimal> FindSmtCommissionAsync(string trnsCode)
    {
        _logger.LogDebug("💰 Fetching SMT commission for transaction: {TrnsCode}", trnsCode);
        return await _agentRepository.FindSmtCommissionAsync(trnsCode);
    }

    public async Task<string> FindTrnsCodeFromSmtTransactionsAsync(string referenceNumber)
    {
        _logger.LogDebug("🔗 Resolving TrnsCode from reference number: {ReferenceNumber}", referenceNumber);
        return await _agentRepository.FindTrnsCodeFromSmtTransactionsAsync(referenceNumber);
    }

    public async Task<string> FindServiceCodeUsingBankServiceTypeAsync(string serviceCode)
    {
        _logger.LogDebug("🔗 Resolving serviceCode from reference number: {ServiceCode}", serviceCode);
        var code = await _agentRepository.FindServiceCodeUsingBankServiceTypeAsync(serviceCode);
        return code?.ToString() ?? serviceCode;
    }

    public async Task<string> ValidateTransactionAsync(string partnerReference)
    {
        _logger.LogDebug("✅ Validating transaction reference: {PartnerReference}", partnerReference);
        return await _agentRepository.ValidateTransactionAsync(partnerReference);
    }

    public async Task<string> ValidateTransactionStatusAsync(string authNumber)
    {
        _logger.LogDebug("📥 Validating transaction status for authNumber: {AuthNumber}", authNumber);
        return await _agentRepository.ValidateTransactionStatusAsync(authNumber);
    }

    public async Task<long> InsertPartnerTransactionAsync(PartnerTransaction partnerTransaction)
    {
        _logger.LogDebug("📌 Inserting PartnerTransaction: {@PartnerTransaction}", partnerTransaction);
        return await _agentRepository.InsertPartnerTransactionAsync(partnerTransaction);
    }

    public async Task<long> UpdateAfterPayingTransactionAsync(string trnsCode, string newStatus)
    {
        _logger.LogInformation("🔁 Updating transaction [{TrnsCode}] to status [{NewStatus}]", trnsCode, newStatus);
        return await _agentRepository.UpdateAfterPayingTransactionAsync(trnsCode, newStatus);
    }

    public async Task<InternalExchangeRateResult> GetExchangeRateAsync(ExchangeRateQueryDto exchangeRateQuery)
    {
        _logger.LogDebug("💱 Fetching exchange rate for: {@Query}", exchangeRateQuery);
        var rate = await _agentRepository.GetExchangeRateAsync(exchangeRateQuery);

        if (rate == null)
        {
            _logger.LogWarning("⚠️ No exchange rate found for query: {@Query}", exchangeRateQuery);
            return null;
        }

        return ExchangeResponseMapper.MapToExchangeRateResponse(rate);
    }

    public async Task<int> GetLocIdForCashPickupServiceAsync(string serviceCode)
    {
        _logger.LogDebug("📍 Fetching LocId for LocationService with ServiceCode={ServiceCode}", serviceCode);
        var result = await _agentRepository.GetLocIdForServiceCodeAsync(serviceCode);

        if (result == null)
        {
            throw new InvalidOperationException($"ServiceCode not found '{serviceCode}'");
        }
        
        return result.Value;
    }
}
