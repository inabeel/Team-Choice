using System.ComponentModel.DataAnnotations;
using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Contracts.DTOs.Transactions;

public class TransactionRequestDto
{
    public string? TransactionId { get; set; }

    [Required(ErrorMessage = "partnerReference is required")]
    public string? PartnerReference { get; set; }

    [Required(ErrorMessage = "timestamp is required")]
    public DateTimeOffset Timestamp { get; set; }

    [Required(ErrorMessage = "purpose is required")]
    public string? Purpose { get; set; }

    [MaxLength(255)]
    public string? Remarks { get; set; }

    [Required(ErrorMessage = "relationship is required")]
    public string Relationship { get; set; }

    public string? EmployeeId { get; set; }

    [Required(ErrorMessage = "payment is required")]
    public PaymentDto? Payment { get; set; }

    [Required(ErrorMessage = "sender is required")]
    public PersonDto? Sender { get; set; }

    [Required(ErrorMessage = "recipient is required")]
    public TransactionRecipientDto? Recipient { get; set; }

    [Required(ErrorMessage = "sendingLocation is required")]
    public LocationDto? SendingLocation { get; set; }

    public Exchanges.PayeeLocationDto? PayeeLocation { get; set; }
}

public class PaymentDto
{
    public string ServiceType { get; set; }
    public string ServiceName { get; set; }
    public string ServiceCode { get; set; }
    public string RecipientCountry { get; set; }
    public string RecipientCurrency { get; set; }
    public string PaymentMode { get; set; }
    public decimal SenderAmount { get; set; }
    public string SenderCurrency { get; set; }
    public decimal RecipientAmount { get; set; }
    public decimal ExchangeRate { get; set; }
    public string Fees { get; set; }
    public int SendingAmount { get; set; }
}

public class SenderDto
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string MobilePhone { get; set; }
    public IdentityDocumentDto IdentityDocument { get; set; }
    public AddressDto Address { get; set; }
    public string Mobile { get; set; }
}

public class TransactionRecipientDto
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string MobilePhone { get; set; }
    public IdentityDocumentDto IdentityDocument { get; set; }
    public AddressDto Address { get; set; }

}

public class SendingLocationDto
{
    public string LocationId { get; set; }
    public string EmployeeId { get; set; }
}

public class PayeeLocationDto
{
    public decimal Share { get; set; }
}

public class IdentityDocumentDto
{
    public string? DocumentType { get; set; }
    public string? DocumentNumber { get; set; }
    public string? ExpirationDate { get; set; }
    public string? CountryRegion { get; set; }
    public string? CountryOfOrigin { get; set; }
    public string? DateOfBirth { get; set; }
}

public class AddressDto
{
    public string? Street { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }
}