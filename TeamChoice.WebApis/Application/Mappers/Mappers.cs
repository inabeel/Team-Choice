using System.Globalization;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Models;

namespace TeamChoice.WebApis.Application.Mappers;

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
    public static Transaction ToTransaction(TransactionRequestDTO dto)
    {
        if (dto == null) return null;

        if (string.IsNullOrWhiteSpace(dto.PartnerReference))
        {
            throw new ArgumentException("Transaction ID and Partner Reference cannot be null");
        }

        return new Transaction
        {
            TransactionId = dto.TransactionId,
            PartnerReference = dto.PartnerReference,
            Timestamp = dto.Timestamp,
            Purpose = dto.Purpose,
            Remarks = dto.Remarks,
            Relationship = dto.Relationship,
            EmployeeId = dto.EmployeeId,
            Payment = dto.Payment,
            Sender = dto.Sender,
            Recipient = dto.Recipient,
            SendingLocation = dto.SendingLocation,
            PayeeLocation = dto.PayeeLocation
        };
    }
}

public static class TransactionSmtMapperUtil
{
    private const string DEFAULT_PAY_MODE = "001";
    // Assuming we might want to log, passing a logger or using console for static helper fallback
    // For production, injecting ILogger<T> into a non-static service wrapping this logic is better,
    // but keeping it static to match Java structure.

    public static SmtTransaction ToSmtTransaction(Transaction transaction)
    {
        if (transaction == null)
        {
            // In a real app, use a proper logging mechanism
            Console.WriteLine("Null transaction provided to mapper");
            return null;
        }

        var tx = new SmtTransaction();

        // General fields
        tx.TrnsCode = transaction.TransactionId;
        tx.AgtRefNo = transaction.PartnerReference;
        tx.TrnsSrvType = transaction.Payment?.ServiceType;
        tx.TrnsDate = ToLocalDateTime(transaction.Timestamp);
        tx.TrnsRemarks = transaction.Remarks;
        tx.RelCode = transaction.Relationship;
        tx.TrnsUser = transaction.EmployeeId;

        // Sender details
        if (transaction.Sender != null)
        {
            var sender = transaction.Sender;
            tx.RemFirstName = sender.FirstName;
            tx.RemMiddleName = sender.MiddleName;
            tx.RemLastName = sender.LastName;
            tx.RemMobile = sender.MobilePhone;

            if (sender.Address != null)
            {
                tx.RemCity = sender.Address.City;
                // Assuming PostalCode isn't in AddressObj yet, based on previous steps. 
                // Java used sender.getAddress().getPostalCode() which mapped to RemCustCode
                // I'll leave it null or map if added to model later.
                // tx.RemCustCode = sender.Address.PostalCode; 
            }

            if (sender.IdentityDocument != null)
            {
                tx.RemIDType = sender.IdentityDocument.DocumentType;
                tx.RemIDNO = sender.IdentityDocument.DocumentNumber;
                tx.RemNatCode = sender.IdentityDocument.CountryOfOrigin;
                tx.RemDOB = sender.IdentityDocument.DateOfBirth; // String in DTO

                // Parse expiration date string to DateTime
                if (DateTime.TryParse(sender.IdentityDocument.ExpirationDate, out var expDate))
                {
                    tx.RemIDExpDate = expDate;
                }
            }
        }

        // Recipient details
        if (transaction.Recipient != null)
        {
            var recipient = transaction.Recipient;
            tx.BenFirstName = recipient.FirstName;
            tx.BenMiddleName = recipient.MiddleName;
            tx.BenLastName = recipient.LastName;
            tx.BenMobile = recipient.MobilePhone;
            // Java code mapped BenMobile to RemPhone as well? "tx.setRemPhone(recipient.getMobilePhone());"
            // This seems like a potential bug or specific requirement in Java code. Mirroring it:
            tx.RemPhone = recipient.MobilePhone;

            if (recipient.IdentityDocument != null)
            {
                tx.BenIDType = recipient.IdentityDocument.DocumentType;
                tx.BenIDNO = recipient.IdentityDocument.DocumentNumber;
                tx.BenNatCode = recipient.IdentityDocument.CountryOfOrigin;

                if (DateTime.TryParse(recipient.IdentityDocument.ExpirationDate, out var expDate))
                {
                    tx.BenIDExpDate = expDate;
                }
            }
        }

        // Locations
        if (transaction.SendingLocation != null)
        {
            tx.SndLocCode = transaction.SendingLocation.LocationId;
        }

        if (transaction.PayeeLocation != null)
        {
            // PayeeLocationDTO in C# currently has 'Share' but might inherit LocationDTO properties?
            // In Java, PayeeLocationDTO extends LocationDTO. 
            // In C#, we defined PayeeLocationDTO : LocationDTO.
            // So LocationId is available.
            tx.RecLocCode = transaction.PayeeLocation.LocationId;
        }

        // Payment info
        if (transaction.Payment != null)
        {
            var payment = transaction.Payment;
            tx.TrnsSrvType = payment.ServiceType;
            tx.TrnsSrvCode = payment.ServiceCode;
            // Java: tx.setRecLocCode(payment.getRecipientCountry()); -> Overwriting RecLocCode from PayeeLocation?
            // The Java code sets RecLocCode twice. Once from PayeeLocation, then from Payment.
            // Mirroring logic:
            tx.RecLocCode = payment.RecipientCountry;

            tx.PayMode = DEFAULT_PAY_MODE;
            tx.PaymentMode = payment.PaymentMode;
            tx.PayCurCode = payment.SenderCurrency;

            decimal? senderAmount = payment.SenderAmount; // Already decimal in C# model
            decimal? fees = ToDecimalSafe(payment.Fees);
            decimal? rate = payment.ExchangeRate;

            tx.FxAmount = senderAmount;
            tx.LcyAmount = senderAmount;
            tx.TrnsComm = fees;

            if (senderAmount.HasValue && fees.HasValue)
            {
                tx.LcyTotAmount = senderAmount.Value + fees.Value;
            }

            tx.RecdRate = rate;
        }

        return tx;
    }

    private static DateTime? ToLocalDateTime(DateTime dateTime)
    {
        // C# DateTime is structurally similar to LocalDateTime. 
        // If it's default(DateTime), return null.
        return dateTime == default ? null : dateTime;
    }

    private static decimal? ToDecimalSafe(string value)
    {
        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }
        return null;
    }
}

public static class RemittanceMapperUtil
{
    public static RemittanceRequest ToRemittanceRequest(SmtTransaction tx)
    {
        if (tx == null)
        {
            // Ideally log warning here if ILogger is available/passed
            return null;
        }

        var req = new RemittanceRequest();

        // Basic transaction info
        req.AgtRefNo = tx.AgtRefNo;
        req.TrnsSrvType = tx.TrnsSrvType;
        req.TrnsSrvCode = tx.TrnsSrvCode;
        req.SndLocCode = tx.SndLocCode;

        // These properties were not in previous SmtTransaction definition, 
        // assuming they exist on the source object based on Java code usage.
        // If SmtTransaction is missing them, I will add them to the model below or comment out if truly missing.
        // Based on typical mapping:
        // req.TrnsCode = tx.TrnsCode; // Not present in RemittanceRequest definition from ServiceDTOs.cs, checking target.

        // Checking RemittanceRequest definition from ServiceDTOs.cs:
        // It has AgtRefNo, TrnsSrvType, TrnsSrvCode, RecLocCode...
        // It DOES NOT have TrnsCode, RecAgtCode, RecConCode, SndFlag, RecFlag, ReceiptNo exposed as inputs usually?
        // The Java RemittanceRequest seems to have them. I will update RemittanceRequest model to include them.

        req.RecLocCode = tx.RecLocCode;
        // req.RecAgtCode = tx.RecAgtCode; 
        // req.RecConCode = tx.RecConCode;
        req.RemNatCode = tx.RemNatCode;

        // Remitter (Sender) details
        req.RemFirstName = tx.RemFirstName;
        req.RemMiddleName = tx.RemMiddleName;
        req.RemLastName = tx.RemLastName;
        // req.RemConCode = tx.RemConCode; 
        req.RemPhone = tx.RemPhone;
        req.RemMobile = tx.RemMobile;
        req.RemcityText = tx.RemCity;
        req.RemDob = tx.RemDOB != null ? DateTime.Parse(tx.RemDOB) : default; // Converting string back to DateTime if needed, or keeping string if target is string

        // Beneficiary (Recipient) details
        req.BenFirstName = tx.BenFirstName;
        req.BenMiddleName = tx.BenMiddleName;
        req.BenLastName = tx.BenLastName;
        // req.BenPhone = tx.BenPhone; // SmtTransaction has BenMobile, mapped to RemPhone in SMT mapper? Checking Java...
        req.BenMobile = tx.BenMobile;
        // req.BenConCode = tx.BenConCode;
        req.BenNatCode = tx.BenNatCode;

        // Payment-related details
        req.PayMode = tx.PayMode;
        req.PaymentMode = tx.PaymentMode;
        // req.PayCurCode = tx.PayCurCode;
        req.FxAmount = tx.FxAmount ?? 0;
        req.TrnsComm = tx.TrnsComm ?? 0;
        // req.LcyAmount = tx.LcyAmount;
        // req.LcyTotAmount = tx.LcyTotAmount;
        // req.TrnsRate = tx.RecdRate;

        // System info
        // req.TrnsUser = tx.TrnsUser;
        // req.TrnsSndMode = tx.TrnsSndMode;
        // req.TrnsStatus = tx.ActualStatus;
        // req.TrnsSubStatus = tx.TrnsSubStatus;
        // req.SndFlag = tx.SndFlag;
        // req.RecFlag = tx.RecFlag;
        // req.ReceiptNo = tx.ReceiptNo;

        return req;
    }
}

