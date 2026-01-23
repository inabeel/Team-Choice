using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TeamChoice.WebApis.Domain.Models.DTOs;

public record TransactionRequestDto
{
    public string? TransactionId { get; init; }
    public string PartnerReference { get; init; } = default!;
    public DateTime Timestamp { get; init; }

    public string Purpose { get; init; } = default!;
    public string Remarks { get; init; } = default!;
    public string Relationship { get; init; } = default!;
    public string? EmployeeId { get; init; }

    public PaymentDto Payment { get; init; } = default!;
    public PersonDto Sender { get; init; } = default!;
    public PersonDto Recipient { get; init; } = default!;
    public LocationDto SendingLocation { get; init; } = default!;
    public LocationDto? PayeeLocation { get; init; }
    public string? TawakalTxnRef { get;  set; }
}

public record TransactionStatusRequestDto(
    string TransactionReference
)
{
    public string? TawakalTxnRef { get;  set; }
}

public record CancelTransactionRequestDto(
    string TawakalTxnRef,
    string LocationCode,
    string Reason
);

public record TransactionResultDto
{
    public string? ProfileId { get; init; }
    public string? ReceiptNo { get; init; }
    public string? Id { get; init; }
    public string? BenfCode { get; init; }
    public double? Crlmt { get; init; }
    public string? TrnsPin { get; init; }
    public string? Status { get; init; }
    public Dictionary<string, string>? Message { get; init; }
    public string? TawakalTxnRef { get; init; }
    public string? CustCode { get; init; }
}

public record TransactionStatusDto(
    string Code,
    string Message
);

public record LocationDto
{
    public string LocationCode { get; init; } = default!;
    public string? LocationName { get; init; }
    public string? CountryCode { get; init; }
    public string? City { get; init; }
}

public record PersonDto
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string? MiddleName { get; init; }

    public string PhoneNumber { get; init; } = default!;
    public string? Email { get; init; }

    public string? Nationality { get; init; }
    public string? IdentificationType { get; init; }
    public string? IdentificationNumber { get; init; }
}

public record PaymentDto
{
    public string ServiceCode { get; init; } = default!;
    public double SendingAmount { get; init; }
    public string SendingCurrency { get; init; } = default!;
    public double? RecipientAmount { get; init; }
    public string RecipientCurrency { get; init; } = default!;

    public double? ExchangeRate { get; init; }
    public double? Fee { get; init; }
}

public class AccountsLookupRequest
{
    /// <summary>
    /// Service code
    /// </summary>
    /// <example>00014</example>
    public string ServiceCode { get; set; } = default!;

    /// <summary>
    /// Phone number in E.164 format
    /// </summary>
    /// <example>+254790715176</example>
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Optional payment mode appended for special services
    /// </summary>
    /// <example>MPESA</example>
    public string? PaymentMode { get; set; }
    public string? ServiceMode { get;  set; }
}

