using System.Globalization;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Models;

namespace TeamChoice.WebApis.Application.Mappers
{
    public static class SendAndTransactionMapper
    {
        // Ideally logger would be injected, but static helpers often use a passed logger or rely on caller context.
        // For simplicity in conversion, we'll omit internal logging or accept a logger if critical.

        public static TransactionRequest ToTransactionRequest(
            TransactionRequestDTOCopy requestDTO,
            decimal share,
            string trnsCode,
            decimal smtComm,
            OutboundProviderCredential outbound,
            string callbackUrl)
        {
            if (requestDTO == null)
            {
                return null;
            }

            var transactionRequest = new TransactionRequest();

            // Top-level fields
            transactionRequest.TransactionId = trnsCode;
            transactionRequest.Timestamp = requestDTO.Timestamp != default ? requestDTO.Timestamp : null;
            transactionRequest.Purpose = null; // TODO: Resolve purpose code
            transactionRequest.Remarks = requestDTO.Remarks;
            transactionRequest.Relationship = null; // TODO: Resolve relationship code
            transactionRequest.CallBackUrl = callbackUrl;

            // Nested objects
            transactionRequest.Payment = MapPayment(requestDTO, smtComm);
            transactionRequest.Sender = MapSender(requestDTO);
            transactionRequest.Recipient = MapRecipient(requestDTO);
            transactionRequest.SendingLocation = MapSendingLocation(requestDTO);
            transactionRequest.PayeeLocation = MapPayeeLocation(share);

            transactionRequest.OutboundProviderCredential = outbound;

            return transactionRequest;
        }

        private static TransactionRequest.PaymentObj MapPayment(TransactionRequestDTOCopy requestDTO, decimal smtComm)
        {
            var payment = new TransactionRequest.PaymentObj();
            if (requestDTO.Payment == null)
            {
                return payment;
            }

            // Note: Java DTO structure had ServiceType/ServiceName on PaymentDTO, 
            // but previous C# DTO defined PaymentDTO with minimal fields (ServiceCode, Amount, CurrencyCode).
            // I will map what is available or infer.
            // Assuming TransactionRequest.PaymentObj has these fields.

            payment.ServiceCode = requestDTO.Payment.ServiceCode;

            // Logic from Java: Determine recipient country
            // Assuming RecipientCountry exists on PaymentDTO (in Java it did)
            // If not, try to get from Recipient address
            string rcCountry = null; // requestDTO.Payment.RecipientCountry
            if (string.IsNullOrWhiteSpace(rcCountry))
            {
                //rcCountry = requestDTO.Recipient?.Address?.Street.Split(',').Length > 2
                //    ? requestDTO.Recipient.Address // Fallback if structure differs
                //    : null;
                // In Java: requestDTO.getRecipient().getAddress().getCountry()
                // C# PersonDTO.Address is a string currently. If complex, needs update.
            }
            // payment.RecipientCountry = rcCountry;
            payment.RecipientCurrency = "USD";

            // payment.PaymentMode = requestDTO.Payment.PaymentMode;
            // payment.SenderAmount = requestDTO.Payment.SenderAmount;

            string senderCurrency = requestDTO.Payment.CurrencyCode;
            payment.SenderCurrency = !string.IsNullOrWhiteSpace(senderCurrency) ? senderCurrency : "USD";

            // payment.RecipientAmount = requestDTO.Payment.RecipientAmount;
            // payment.ExchangeRate = requestDTO.Payment.ExchangeRate;
            payment.Fees = smtComm.ToString(CultureInfo.InvariantCulture);

            return payment;
        }

        private static TransactionRequest.SenderObj MapSender(TransactionRequestDTOCopy requestDTO)
        {
            var sender = new TransactionRequest.SenderObj();
            if (requestDTO.Sender == null) return sender;

            sender.FirstName = requestDTO.Sender.FirstName;
            sender.MiddleName = requestDTO.Sender.MiddleName;
            sender.LastName = requestDTO.Sender.LastName;
            sender.MobilePhone = requestDTO.Sender.Mobile;

            // Identity Document mapping would go here if present in DTO

            // Address mapping
            //if (!string.IsNullOrEmpty(requestDTO.Sender.Address))
            //{
                var addr = new TransactionRequest.AddressObj();
                addr.Street = requestDTO?.Sender?.Address?.Street;
                // C# PersonDTO currently has simple string Address/City. 
                // Adapting to structure defined in TransactionRequest model.
                sender.Address = addr;
            //}

            return sender;
        }

        private static TransactionRequest.RecipientObj MapRecipient(TransactionRequestDTOCopy requestDTO)
        {
            var recipient = new TransactionRequest.RecipientObj();
            if (requestDTO.Recipient == null) return recipient;

            recipient.FirstName = requestDTO.Recipient.FirstName;
            recipient.MiddleName = requestDTO.Recipient.MiddleName;
            recipient.LastName = requestDTO.Recipient.LastName;
            recipient.MobilePhone = requestDTO.Recipient.MobilePhone;

            //if (!string.IsNullOrEmpty(requestDTO.Recipient.Address))
            //{
                var addr = new TransactionRequest.AddressObj();
                addr.Street = requestDTO?.Recipient?.Address?.Street;
                recipient.Address = addr;
            //}

            return recipient;
        }

        private static TransactionRequest.SendingLocationObj MapSendingLocation(TransactionRequestDTOCopy requestDTO)
        {
            var sl = new TransactionRequest.SendingLocationObj();
            if (requestDTO.SendingLocation != null)
            {
                sl.LocationId = requestDTO.SendingLocation.LocationId;
            }
            sl.EmployeeId = requestDTO.EmployeeId;
            return sl;
        }

        private static TransactionRequest.PayeeLocationObj MapPayeeLocation(decimal share)
        {
            var pl = new TransactionRequest.PayeeLocationObj();
            pl.Share = share;
            return pl;
        }
    }
}
