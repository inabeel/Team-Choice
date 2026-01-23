using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Domain.Models.DTOs
{
    public class SMTMstMpUser {
        public string MpName { get; set; }
        public string OrgCode { get; set; }
        public string MpUserId { get; set; }
        public string MpCode { get; set; }
        public string UserId { get; set; }
        public string Pin { get; set; }
        public string AgentCode { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string Password3 { get; set; }
        public string LocCode { get; set; }
        public string SubCode { get; set; }
        public string CmpCode { get; set; }
        public string ActiveFlag { get; set; }
        public string Url { get; set; }
        public string MpAgentCode { get; set; }
        public string AddInfo1 { get; set; }
        public string AddInfo2 { get; set; }
        public string AddInfo3 { get; set; }
        public string AddInfo4 { get; set; }
        public string AddInfo5 { get; set; }
        public string IsRate { get; set; }
    }

    public class OutboundProviderCredential
    {
        public int? ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }
        public string BaseURL { get; set; }
    }

    public class ExchangeRateResult
    {
        public decimal EtbIr { get; set; }
        public decimal Usd { get; set; }
    }

    public class ExchangeRateQuery
    {
        public string CurCode { get; set; }
        public string AgtCode { get; set; }
        public string LocCode { get; set; }
    }

    public sealed class PartnerTransaction
    {
        public string? TransactionDate { get; init; }
        public string? PartnerReference { get; init; }
        public string? PartnerCode { get; init; }
        public string Status { get; init; } = default!;
        public string? Payload { get; init; }
    }

    public class TransactionRequestDTOCopy
    {
        public PaymentInfo Payment { get; set; }
    }

    public class PaymentInfo
    {
        public string ServiceCode { get; set; }
    }

    public class TransactionStatusDTO { }

    public class TransactionRequest { }

    public class RemittanceResultDTO
    {
        public string Reference { get; set; }
        public string ProfileId { get; set; }
        public string ReceiptNo { get; set; }
        public string Id { get; set; }
        public string BenfCode { get; set; }
        public decimal Crlmt { get; set; }
        public string TrnsPin { get; set; }
        public string CustCode { get; set; }
    }

    public class RemittanceRequest
    {
        public string Id { get; set; }
        public string AgtRefNo { get; set; }
        public string TrnsSrvType { get; set; }
        public string TrnsSrvCode { get; set; }
        public string RecLocCode { get; set; }
        public string RemFirstName { get; set; }
        public string RemMiddleName { get; set; }
        public string RemLastName { get; set; }
        public string RemNatCode { get; set; }
        public string RemPhone { get; set; }
        public string RemMobile { get; set; }
        public string RemcityText { get; set; }
        public DateTime RemDob { get; set; }
        public string BenFirstName { get; set; }
        public string BenMiddleName { get; set; }
        public string BenLastName { get; set; }
        public string BenNatCode { get; set; }
        public string BenMobile { get; set; }
        public string PayMode { get; set; }
        public decimal FxAmount { get; set; }
        public decimal TrnsComm { get; set; }
        public string PaymentMode { get; set; }
    }

    public class SmtTransaction
    {
        public string TrnsStatus { get; set; }
        public string TrnsSubStatus { get; set; }
        public string ActualStatus { get; set; }
        public string AgtRefNo { get; set; }
    }

    public class Transaction { }

    public class TeamsNotificationRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CreatedUtc { get; set; }
        public string ViewUrl { get; set; }
        public List<Dictionary<string, string>> Properties { get; set; }
    }

    public class AccountLookupResponse
    {
        public string Response { get; set; }
        public string ServiceType { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceMode { get; set; }
        public string AccountName { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class CancelReceiveRequest
    {
        public string Refno { get; set; }
        public string Reason { get; set; }
        public string Agtcode { get; set; }
        public string Subcode { get; set; }
        public string Loccode { get; set; }
        public string Rqstuserid { get; set; }
        public DateTime Rqstdate { get; set; }
        public string Agtaprvduser { get; set; }
        public DateTime Agtaprvddate { get; set; }
        public string Smtaprvduser { get; set; }
        public DateTime Smtaprvddate { get; set; }
        public decimal Refundrate { get; set; }
        public string Recagtcode { get; set; }
        public decimal Refundamt { get; set; }
        public string Refundackflg { get; set; }
        public string Refundackuser { get; set; }
        public DateTime Refundackdate { get; set; }
        public string Refundfrom { get; set; }
        public string Module { get; set; }
        public string Rateoption { get; set; }
        public string Commoption { get; set; }
        public string Commdesc { get; set; }
        public string Trnsstatus { get; set; }
        public string Trnssubstatus { get; set; }
        public string Agenttype { get; set; }
        public string Action { get; set; }
        public string Bmapruser { get; set; }
        public string Errorsource { get; set; }
        public string Trnssndmode { get; set; }

        // From Controller usage
        public string TawakalTxnRef { get; set; } // Likely aliases to Refno
    }

    public class TransactionStatusCallback
    {
        public string TransactionId { get; set; }
        public string TransactionTimestamp { get; set; }
    }
}

namespace YourNamespace.Repositories
{
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
        Task<ExchangeRateResult> GetExchangeRateAsync(ExchangeRateQuery query);
        Task<int?> GetLocIdForServiceCodeAsync(string serviceCode);
    }

    public interface IDatabaseClient
    {
        Task<T> ExecuteStoredProcedureAsync<T>(string procedureName, Dictionary<string, object> parameters);
        Task<decimal> QueryOneAsync(string fIND_RECEIVE_COMMISSION_BY_AGENT_CODE, Dictionary<string, object> parameters, Func<object, decimal> value);
    }
}

namespace YourNamespace.Utils
{
    public static class TransactionPayloadMapper
    {
        public static TransactionRequest ToTransactionRequest(TransactionRequestDTOCopy dto, decimal rcvComm, string trnsCode, decimal smtComm, OutboundProviderCredential cred, string callbackUrl)
        {
            return new TransactionRequest();
        }
    }

    public static class TransactionUtil
    {
        public static bool ValidateSenderLocation(string locId) => true;
    }

    public static class TransactionMapperUtil
    {
        public static Transaction ToTransaction(TransactionRequestDto dto) => new Transaction();
    }

    public static class TransactionSmtMapperUtil
    {
        public static SmtTransaction ToSmtTransaction(Transaction txn) => new SmtTransaction();
    }

    public static class RemittanceMapperUtil
    {
        public static RemittanceRequest ToRemittanceRequest(SmtTransaction txn) => new RemittanceRequest();
    }

    public static class RemittanceSql
    {
        public const string CALL_PROCEDURE = "YOUR_STORED_PROCEDURE_NAME";
    }

    public static class CancellationSql
    {
        public const string CALL_PROCEDURE = "YOUR_CANCEL_PROCEDURE_NAME";
    }

    public static class RemittanceConstants
    {
        public const string CURRENCY_CODE = "USD";
        public const string REC_COUNTRY_CODE = "SO";
        public const string REM_COUNTRY_CODE = "US";
        public const string BEN_COUNTRY_CODE = "SO";
        public const decimal TOTAL_AMOUNT = 0;
        public const decimal RATE = 1;
        public const decimal RATE_DIV = 1;
        public const string DEFAULT_RECEIPT_NO = "00000";
        public const string ERROR_SOURCE = "API";
    }
}
