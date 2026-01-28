using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Domain.Exceptions;

namespace TeamChoice.WebApis.Application.Validators;

public interface ITransactionValidator
{
    void Validate(TransactionRequestDto request);
}

/// <summary>
/// Enforces domain-level validation rules for transactions.
/// </summary>
public sealed class TransactionValidator : ITransactionValidator
{
    public void Validate(TransactionRequestDto request)
    {
        if (request is null)
            throw new TransactionValidationException("Transaction request cannot be null");

        ValidateCoreFields(request);
        ValidatePayment(request.Payment);
        ValidateSender(request.Sender);
        ValidateRecipient(request.Recipient);
        ValidateLocation(request.SendingLocation);
    }

    // --------------------------------------------------------------------
    // Validation Rules
    // --------------------------------------------------------------------

    private static void ValidateCoreFields(TransactionRequestDto request)
    {
        Require(request.PartnerReference, "PartnerReference");
        Require(request.Purpose, "Purpose");
        Require(request.Relationship, "Relationship");
    }

    private static void ValidatePayment(PaymentDto payment)
    {
        if (payment is null)
            throw new TransactionValidationException("Payment details are required");

        Require(payment.ServiceCode, "Payment.ServiceCode");

        if (payment.SendingAmount <= 0)
            throw new TransactionValidationException("Sending amount must be greater than zero");
    }

    private static void ValidateSender(PersonDto sender)
    {
        if (sender is null)
            throw new TransactionValidationException("Sender details are required");

        Require(sender.FirstName, "Sender.FirstName");
        Require(sender.LastName, "Sender.LastName");
        Require(sender.PhoneNumber, "Sender.PhoneNumber");
    }

    private static void ValidateRecipient(PersonDto recipient)
    {
        if (recipient is null)
            throw new TransactionValidationException("Recipient details are required");

        Require(recipient.FirstName, "Recipient.FirstName");
        Require(recipient.LastName, "Recipient.LastName");
        Require(recipient.PhoneNumber, "Recipient.PhoneNumber");
    }

    private static void ValidateLocation(LocationDto location)
    {
        if (location is null)
            throw new TransactionValidationException("Sending location is required");

        Require(location.LocationCode, "SendingLocation.LocationCode");
    }

    // --------------------------------------------------------------------
    // Helpers
    // --------------------------------------------------------------------

    private static void Require(string? value, string fieldName)
    {
        //if (string.IsNullOrWhiteSpace(value))
        //    throw new TransactionValidationException(
        //        $"Field '{fieldName}' is required");
    }
}

