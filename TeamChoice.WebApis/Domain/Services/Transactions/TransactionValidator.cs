using TeamChoice.WebApis.Application.Commands.Transactions;
using TeamChoice.WebApis.Domain.Exceptions;

namespace TeamChoice.WebApis.Domain.Services.Transactions;

public sealed class TransactionValidator
{
    public void ValidateCreate(CreateTransactionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        ValidatePartnerReference(command.PartnerReference);
        ValidateAmount(command.Amount);
        ValidateCurrency(command.Currency);
        ValidateServiceCode(command.ServiceCode);
        ValidateSender(command.Sender);
        ValidateReceiver(command.Receiver);
    }

    private static void ValidatePartnerReference(string partnerReference)
    {
        if (string.IsNullOrWhiteSpace(partnerReference))
        {
            throw new TransactionValidationException(
                "Partner reference must not be empty.");
        }
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new TransactionValidationException(
                "Transaction amount must be greater than zero.");
        }
    }

    private static void ValidateCurrency(string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new TransactionValidationException(
                "Currency must be specified.");
        }
    }

    private static void ValidateServiceCode(string serviceCode)
    {
        if (string.IsNullOrWhiteSpace(serviceCode))
        {
            throw new InvalidServiceCodeException(
                "Service code must be provided.");
        }
    }

    private static void ValidateSender(SenderInfo sender)
    {
        if (sender is null)
        {
            throw new TransactionValidationException(
                "Sender information is required.");
        }

        if (string.IsNullOrWhiteSpace(sender.Name))
        {
            throw new TransactionValidationException(
                "Sender name is required.");
        }

        if (!IsValidPhone(sender.PhoneNumber))
        {
            throw new TransactionValidationException(
                "Sender phone number is invalid.");
        }

        if (string.IsNullOrWhiteSpace(sender.LocationId))
        {
            throw new TransactionValidationException(
                "Sender location ID is required.");
        }
    }

    private static void ValidateReceiver(ReceiverInfo receiver)
    {
        if (receiver is null)
        {
            throw new TransactionValidationException(
                "Receiver information is required.");
        }

        if (string.IsNullOrWhiteSpace(receiver.Name))
        {
            throw new TransactionValidationException(
                "Receiver name is required.");
        }

        if (!IsValidPhone(receiver.PhoneNumber))
        {
            throw new TransactionValidationException(
                "Receiver phone number is invalid.");
        }
    }

    private static bool IsValidPhone(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        // Minimal, deterministic, culture-invariant rule.
        // Replace later ONLY if business explicitly changes.
        return phoneNumber.All(char.IsDigit)
               && phoneNumber.Length is >= 8 and <= 15;
    }
}
