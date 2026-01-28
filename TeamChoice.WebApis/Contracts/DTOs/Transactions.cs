using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TeamChoice.WebApis.Contracts.DTOs;

public class TransactionRequestDto
{
    public string? TransactionId { get; set; }
    public string PartnerReference { get; set; } = default!;
    public DateTime Timestamp { get; set; }

    public string Purpose { get; set; } = default!;
    public string Remarks { get; set; } = default!;
    public string Relationship { get; set; } = default!;
    public string? EmployeeId { get; set; }

    public PaymentDto Payment { get; set; } = default!;
    public PersonDto Sender { get; set; } = default!;
    public PersonDto Recipient { get; set; } = default!;
    public LocationDto SendingLocation { get; set; } = default!;
    public LocationDto? PayeeLocation { get; set; }
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

public class LocationDto
{
    public string? LocationCode { get; set; } = default!;
    public string? LocationName { get; set; }
    public string? CountryCode { get; set; }
    public string? City { get; set; }
    public string? LocationId { get;  set; }
}

public record PersonDto
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string? MiddleName { get; init; }

    public string? PhoneNumber { get; init; } = default!;
    public string? Email { get; init; }

    public string? Nationality { get; init; }
    public string? IdentificationType { get; init; }
    public string? IdentificationNumber { get; init; }
}

public class PaymentDto
{
    public string? ServiceCode { get; set; } = default!;
    public double? SendingAmount { get; set; }
    public string? SendingCurrency { get; set; } = default!;
    public double? RecipientAmount { get; set; }
    public string? RecipientCurrency { get; set; } = default!;

    public double? ExchangeRate { get; set; }
    public double? Fee { get; set; }
}

public class AccountsLookupRequest
{
    /// <summary>
    /// Service code
    /// </summary>
    /// <example>00014</example>
    [Required]
    public string ServiceCode { get; set; } = default!;

    /// <summary>
    /// Phone number in E.164 format
    /// </summary>
    /// <example>+254790715176</example>
    [Required]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Optional payment mode appended for special services
    /// </summary>
    /// <example>MPESA</example>
    public string? PaymentMode { get; set; }

    public string? ServiceMode { get;  set; }
}

