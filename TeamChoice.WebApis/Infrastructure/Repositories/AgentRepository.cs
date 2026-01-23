using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Models.DTOs;
using TeamChoice.WebApis.Utils;
using YourNamespace.Repositories;

namespace TeamChoice.WebApis.Infrastructure.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly IDatabaseService _databaseService; // Use the new service
        private readonly ILogger<AgentRepository> _logger;

    public AgentRepository(IDatabaseService databaseService, ILogger<AgentRepository> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task<decimal> FindRcvcomByAgtCodeAsync(string agtCode)
    {
        var parameters = new Dictionary<string, object> { { "@agtCode", agtCode } };

        return await _databaseService.QueryOneAsync(
            AgentSqlQueries.FIND_RECEIVE_COMMISSION_BY_AGENT_CODE,
            parameters,
            reader => Convert.ToDecimal(reader["AMOUNT"])
        );
    }

    public async Task<OutboundProviderCredential> FindOutboundCredAsync(string serviceCode)
    {
        var parameters = new Dictionary<string, object> { { "@serviceCode", serviceCode } };

        return await _databaseService.QueryOneAsync(
            AgentSqlQueries.FIND_OUTBOUND_CREDENTIALS_BY_SERVICE_CODE,
            parameters,
            reader => new OutboundProviderCredential
            {
                ProviderId = reader["ProviderId"] != DBNull.Value ? Convert.ToInt32(reader["ProviderId"]) : (int?)null,
                ProviderName = reader["ProviderName"] as string,
                ServiceCode = reader["ServiceCode"] as string,
                ServiceName = reader["ServiceName"] as string,
                Username = reader["Username"] as string,
                Password = reader["Password"] as string,
                SecretKey = reader["SecretKey"] as string,
                BaseURL = reader["BaseURL"] as string
            }
        );
    }

    public async Task<ExchangeRateResult> GetExchangeRateAsync(ExchangeRateQuery query)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@curcode", query.CurCode },
            { "@agtcode", query.AgtCode },
            { "@loccode", query.LocCode }
        };

        return await _databaseService.QueryOneAsync(
            AgentSqlQueries.GET_EXCHANGE_RATE,
            parameters,
            reader => new ExchangeRateResult
            {
                EtbIr = Convert.ToDecimal(reader["etbIr"]),
                Usd = Convert.ToDecimal(reader["usd"])
            }
        );
    }

    public async Task<decimal> FindSmtCommissionAsync(string agtCode)
    {
        var parameters = new Dictionary<string, object> { { "@agtCode", agtCode } };

        return await _databaseService.QueryOneAsync(
            AgentSqlQueries.FIND_SMT_COMMISSION_BY_TRNS_CODE,
            parameters,
            reader => Convert.ToDecimal(reader["SMT_TOTCOM"])
        );
    }

    public async Task<int?> FindServiceCodeUsingBankServiceTypeAsync(string serviceCode)
    {
        var parameters = new Dictionary<string, object> { { "@serviceCode", serviceCode } };

        return await _databaseService.QueryOneAsync<int?>(
            AgentSqlQueries.FIND_BANK_SERVICETYPE,
            parameters,
            reader => reader["DeliveryTypeID"] != DBNull.Value ? Convert.ToInt32(reader["DeliveryTypeID"]) : (int?)null
        );
    }

    public async Task<string> FindTrnsCodeFromSmtTransactionsAsync(string authNumber)
    {
        var parameters = new Dictionary<string, object> { { "@authNumber", authNumber } };

        return await _databaseService.QueryOneAsync(
            AgentSqlQueries.FIND_TRNS_CODE_FROM_TRANSACTIONS,
            parameters,
            reader => reader["TrnsCode"] as string
        );
    }

    public async Task<string> ValidateTransactionAsync(string partnerReference)
    {
        var parameters = new Dictionary<string, object> { { "@partnerReference", partnerReference } };

        var result = await _databaseService.QueryOneAsync(
            AgentSqlQueries.VALIDATE_TRANSACTION_BY_PARTNER_REFERENCE,
            parameters,
            reader => reader["partnerCode"] as string
        );
        return result ?? "NOT_FOUND";
    }

    public async Task<string> ValidateTransactionStatusAsync(string trnsCode)
    {
        var parameters = new Dictionary<string, object> { { "@trnsCode", trnsCode } };

        var result = await _databaseService.QueryOneAsync(
            AgentSqlQueries.VALIDATE_TRANSACTION_STATUS,
            parameters,
            reader => reader["TrnsStatus"] as string
        );
        return result ?? "NOT_FOUND";
    }

    public async Task<long> InsertPartnerTransactionAsync(PartnerTransaction p)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@partnerReference", p.PartnerReference },
            { "@transactionDate", p.TransactionDate },
            { "@status", p.Status },
            { "@payload", p.Payload },
            { "@partnerCode", p.PartnerCode }
        };

        return await _databaseService.ExecuteNonQueryAsync(AgentSqlQueries.INSERT_PARTNER_TRANSACTION, parameters);
    }

    public async Task<long> UpdateAfterPayingTransactionAsync(string trnsCode, string newStatus)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@trnsStatus", newStatus },
            { "@agtRefNo", trnsCode }
        };

        return await _databaseService.ExecuteNonQueryAsync(AgentSqlQueries.UPDATE_TRANSACTION_STATUS, parameters);
    }

    public async Task<SMTMstMpUser> GetUserByLocCodeAsync(string locCode)
    {
        var parameters = new Dictionary<string, object> { { "@loccode", locCode } };

        return await _databaseService.QueryOneAsync(
            AgentSqlQueries.GET_USER_BY_LOC_CODE,
            parameters,
            reader => new SMTMstMpUser
            {
                MpName = reader["MPNAME"] as string,
                OrgCode = reader["ORGCODE"] as string,
                MpUserId = reader["MPUSERID"] as string,
                MpCode = reader["MPCODE"] as string,
                UserId = reader["USERID"] as string,
                Pin = reader["PIN"] as string,
                AgentCode = reader["AGENTCODE"] as string,
                Password1 = reader["PASSWRD1"] as string,
                Password2 = reader["PASSWRD2"] as string,
                Password3 = reader["PASSWRD3"] as string,
                LocCode = reader["LOCCODE"] as string,
                SubCode = reader["SUBCODE"] as string,
                CmpCode = reader["CMPCODE"] as string,
                ActiveFlag = reader["ACTIVEFLG"] as string,
                Url = reader["URL"] as string,
                MpAgentCode = reader["MPAGENTCODE"] as string,
                AddInfo1 = reader["ADDINFO1"] as string,
                AddInfo2 = reader["ADDINFO2"] as string,
                AddInfo3 = reader["ADDINFO3"] as string,
                AddInfo4 = reader["ADDINFO4"] as string,
                AddInfo5 = reader["ADDINFO5"] as string,
                IsRate = reader["ISRATE"] as string
            }
        );
    }

    public async Task<string> GetRecLocCodeAsync(string country)
    {
        var parameters = new Dictionary<string, object> { { "@country", country } };

        var result = await _databaseService.QueryOneAsync(
            AgentSqlQueries.GET_REGLOCCODE,
            parameters,
            reader => reader["ExternalLocId"] as string
        );
        return result ?? "NOT_FOUND";
    }

    public async Task<int?> GetLocIdForServiceCodeAsync(string serviceCode)
    {
        var parameters = new Dictionary<string, object> { { "@serviceCode", serviceCode } };

        return await _databaseService.QueryOneAsync<int?>(
            AgentSqlQueries.GET_LOCID_BY_SERVICE_CODE,
            parameters,
            reader => reader["LocId"] != DBNull.Value ? Convert.ToInt32(reader["LocId"]) : (int?)null
        );
    }
}