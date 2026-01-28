using System.ComponentModel.DataAnnotations;
using TeamChoice.WebApis.Contracts.DTOs;
using static TeamChoice.WebApis.Domain.Models.TransactionRequest;

namespace TeamChoice.WebApis.Contracts.Exchanges;

public class SMTMstMpUser
{
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

public sealed class ExchangeRateQueryDto
{
    public string AgentCode { get; init; } = default!;
    public string LocationCode { get; init; } = default!;
    public string CurrencyCode { get; init; } = default!;
}

public sealed class PartnerTransaction
{
    public string? TransactionDate { get; init; }
    public string? PartnerReference { get; init; }
    public string? PartnerCode { get; init; }
    public string Status { get; init; } = default!;
    public string? Payload { get; init; }
}
public class TransactionRequestDTO
{
    public string? TransactionId { get; set; }

    [Required(ErrorMessage = "partnerReference is required")]
    public string? PartnerReference { get; set; }

    [Required(ErrorMessage = "timestamp is required")]
    public DateTime Timestamp { get; set; }

    [Required(ErrorMessage = "purpose is required")]
    public string? Purpose { get; set; }

    [MaxLength(255)]
    public string? Remarks { get; set; }

    [Required(ErrorMessage = "relationship is required")]
    public string Relationship { get; set; }

    public string? EmployeeId { get; set; }

    [Required(ErrorMessage = "payment is required")]
    public PaymentObj? Payment { get; set; }

    [Required(ErrorMessage = "sender is required")]
    public PersonDto? Sender { get; set; }

    [Required(ErrorMessage = "recipient is required")]
    public RecipientObj? Recipient { get; set; }

    [Required(ErrorMessage = "sendingLocation is required")]
    public LocationDto? SendingLocation { get; set; }

    public PayeeLocationDto? PayeeLocation { get; set; }
}
public class PayeeLocationDto : LocationDto
{
    public string? Share { get; set; }
}
public class TransactionRequestDTOCopy
{
    public PaymentInfo Payment { get; set; }
    public string Timestamp { get;  set; }
    public RecipientObj Recipient { get;  set; }
    public LocationDto SendingLocation { get;  set; }
    public string EmployeeId { get;  set; }
    public SenderObj Sender { get; set; }
    public string Remarks { get;  set; }
}

public class PaymentInfo
{
    public string ServiceCode { get; set; }
    public string CurrencyCode { get;  set; }
}

public class TransactionStatusDTO
{
    public string TransactionTimestamp { get;  set; }
    public string TransactionId { get;  set; }
    public object Status { get;  set; }
}



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
    public string SndLocCode { get;  set; }
}

public class SmtTransaction
{
    public string TrnsCode { get; set; }
    public string AgtRefNo { get; set; }
    public string TrnsSrvType { get; set; }
    public DateTime? TrnsDate { get; set; }
    public string TrnsRemarks { get; set; }
    public string RelCode { get; set; }
    public string TrnsUser { get; set; }

    // Sender Details
    public string RemFirstName { get; set; }
    public string RemMiddleName { get; set; }
    public string RemLastName { get; set; }
    public string RemMobile { get; set; }
    public string RemCity { get; set; }
    public string RemCustCode { get; set; } // Referenced in comments/Java logic
    public string RemIDType { get; set; }
    public string RemIDNO { get; set; }
    public string RemNatCode { get; set; }
    public string RemDOB { get; set; }
    public DateTime? RemIDExpDate { get; set; }

    // Recipient Details
    public string BenFirstName { get; set; }
    public string BenMiddleName { get; set; }
    public string BenLastName { get; set; }
    public string BenMobile { get; set; }
    public string RemPhone { get; set; } // Mapped from BenMobile in logic
    public string BenIDType { get; set; }
    public string BenIDNO { get; set; }
    public string BenNatCode { get; set; }
    public DateTime? BenIDExpDate { get; set; }

    // Location & Payment
    public string SndLocCode { get; set; }
    public string RecLocCode { get; set; }
    public string TrnsSrvCode { get; set; }
    public string PayMode { get; set; }
    public string PaymentMode { get; set; }
    public string PayCurCode { get; set; }

    // Financials
    public decimal? FxAmount { get; set; }
    public decimal? LcyAmount { get; set; }
    public decimal? TrnsComm { get; set; }
    public decimal? LcyTotAmount { get; set; }
    public decimal? RecdRate { get; set; }

    // Status fields (often used in SMT context)
    public string TrnsStatus { get; set; }
    public string TrnsSubStatus { get; set; }
    public string ActualStatus { get; set; }
}

public class Transaction
{
    public string TransactionId { get; set; }
    public string PartnerReference { get; set; }
    public DateTime Timestamp { get; set; }
    public string Purpose { get; set; }
    public string Remarks { get; set; }
    public string Relationship { get; set; }
    public string EmployeeId { get; set; }

    public PaymentObj Payment { get; set; }
    public PersonDto Sender { get; set; }
    public RecipientObj Recipient { get; set; }
    public LocationDto SendingLocation { get; set; }
    public PayeeLocationDto PayeeLocation { get; set; }
}

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
    public object Status { get;  set; }
}