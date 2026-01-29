using System.ComponentModel.DataAnnotations;
using TeamChoice.WebApis.Contracts.DTOs.Transactions;

namespace TeamChoice.WebApis.Contracts.DTOs;

public class TransactionStatusRequestDto
{
    public string? TransactionReference { get; set; }
    public string? TawakalTxnRef { get;  set; }
}

public record CancelTransactionRequestDto(
    string TawakalTxnRef,
    string LocationCode,
    string Reason
);

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
    public string? MobilePhone { get;  set; }
    public AddressDto? Address { get;  set; }
    public IdentityDocumentDto? IdentityDocument { get;  set; }
}

//public class PaymentDto
//{
//    public string? ServiceCode { get; set; } = default!;
//    public double? SendingAmount { get; set; }
//    public string? SendingCurrency { get; set; } = default!;
//    public double? RecipientAmount { get; set; }
//    public string? RecipientCurrency { get; set; } = default!;

//    public double? ExchangeRate { get; set; }
//    public double? Fee { get; set; }
//    public string ServiceType { get;  set; }
//}

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

