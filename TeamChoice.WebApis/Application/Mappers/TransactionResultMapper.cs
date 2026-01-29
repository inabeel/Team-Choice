using TeamChoice.WebApis.Application.Commands.Transactions;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.DTOs.Transactions;

namespace TeamChoice.WebApis.Application.Mappers;

public sealed class TransactionRequestMapper
{
    public CreateTransactionCommand Map(TransactionRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Payment);
        ArgumentNullException.ThrowIfNull(request.Sender);
        ArgumentNullException.ThrowIfNull(request.Recipient);
        ArgumentNullException.ThrowIfNull(request.SendingLocation);

        return new CreateTransactionCommand(
            PartnerReference: request.PartnerReference!,

            // 🔑 Amount & currency come from PAYMENT
            Amount: request.Payment.SenderAmount,
            Currency: request.Payment.SenderCurrency,
            ServiceCode: request.Payment.ServiceCode,

            // 🔑 Sender info
            Sender: new SenderInfo(
                Name: BuildFullName(
                    request.Sender.FirstName,
                    request.Sender.MiddleName,
                    request.Sender.LastName),
                PhoneNumber: request.Sender.MobilePhone,
                LocationId: request.SendingLocation.LocationId
            ),

            // 🔑 Recipient → Receiver translation
            Receiver: new ReceiverInfo(
                Name: BuildFullName(
                    request.Recipient.FirstName,
                    request.Recipient.MiddleName,
                    request.Recipient.LastName),
                PhoneNumber: request.Recipient.MobilePhone
            )
        );
    }

    public CancelTransactionCommand MapCancel(CancelTransactionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new CancelTransactionCommand(
            TransactionReference: request.TawakalTxnRef,
            LocationCode: request.LocationCode,
            Reason: request.Reason
        );
    }

    public ValidateTransactionStatusCommand MapStatus(
        TransactionStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new ValidateTransactionStatusCommand(
            TransactionReference: request.TawakalTxnRef
        );
    }

    private static string BuildFullName(
        string firstName,
        string? middleName,
        string lastName)
    {
        return string.IsNullOrWhiteSpace(middleName)
            ? $"{firstName} {lastName}"
            : $"{firstName} {middleName} {lastName}";
    }
}
