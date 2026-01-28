using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Domain.Models
{
    public class TransactionRequest
    {
        public string TransactionId { get; set; }
        public string Timestamp { get; set; }
        public string Purpose { get; set; }
        public string Remarks { get; set; }
        public string Relationship { get; set; }
        public string CallBackUrl { get; set; }

        public PaymentObj Payment { get; set; }
        public SenderObj Sender { get; set; }
        public RecipientObj Recipient { get; set; }
        public SendingLocationObj SendingLocation { get; set; }
        public PayeeLocationObj PayeeLocation { get; set; }
        public OutboundProviderCredential OutboundProviderCredential { get; set; }

        public class PaymentObj
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
            public int SendingAmount { get;  set; }
        }

        public class SenderObj
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string MobilePhone { get; set; }
            public IdentityDocumentObj IdentityDocument { get; set; }
            public AddressObj Address { get; set; }
            public string Mobile { get; set; }
        }

        public class RecipientObj
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string MobilePhone { get; set; }
            public IdentityDocumentObj IdentityDocument { get; set; }
            public AddressObj Address { get; set; }
           
        }

        public class SendingLocationObj
        {
            public string LocationId { get; set; }
            public string EmployeeId { get; set; }
        }

        public class PayeeLocationObj
        {
            public decimal Share { get; set; }
        }

        public class IdentityDocumentObj
        {
            public string DocumentType { get; set; }
            public string DocumentNumber { get; set; }
            public string ExpirationDate { get; set; }
            public string CountryRegion { get; set; }
            public string CountryOfOrigin { get; set; }
            public string DateOfBirth { get; set; }
        }

        public class AddressObj
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
        }
    }
}
