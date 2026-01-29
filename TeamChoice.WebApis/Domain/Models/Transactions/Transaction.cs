using TeamChoice.WebApis.Domain.Exceptions;
using TeamChoice.WebApis.Domain.Models.Enums;

namespace TeamChoice.WebApis.Domain.Models.Transactions;

public sealed class Transaction
{
    public string Reference { get; }
    public string PartnerReference { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public string ServiceType { get; }
    public TransactionStatus Status { get; private set; }

    // =========================
    // Creation (NEW transaction)
    // =========================
    private Transaction(
        string reference,
        string partnerReference,
        decimal amount,
        string currency,
        string serviceType,
        TransactionStatus status)
    {
        Reference = reference;
        PartnerReference = partnerReference;
        Amount = amount;
        Currency = currency;
        ServiceType = serviceType;
        Status = status;
    }

    public static Transaction Create(
        string reference,
        string partnerReference,
        decimal amount,
        string currency,
        string serviceType)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            throw new TransactionValidationException(
                "Transaction reference is required.");
        }

        if (string.IsNullOrWhiteSpace(partnerReference))
        {
            throw new TransactionValidationException(
                "Partner reference is required.");
        }

        if (amount <= 0)
        {
            throw new TransactionValidationException(
                "Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new TransactionValidationException(
                "Currency is required.");
        }

        if (string.IsNullOrWhiteSpace(serviceType))
        {
            throw new TransactionValidationException(
                "Service type is required.");
        }

        return new Transaction(
            reference,
            partnerReference,
            amount,
            currency,
            serviceType,
            TransactionStatus.Pending);
    }

    // =========================
    // Rehydration (FROM DB)
    // =========================
    public static Transaction Rehydrate(
        string reference,
        string partnerReference,
        decimal amount,
        string currency,
        string serviceType,
        TransactionStatus status)
    {
        return new Transaction(
            reference,
            partnerReference,
            amount,
            currency,
            serviceType,
            status);
    }

    // =========================
    // Behavior
    // =========================
    public bool CanBeCancelled()
        => Status == TransactionStatus.Pending;

    public void Cancel()
    {
        if (!CanBeCancelled())
        {
            throw new AlreadyPaidException(Reference);
        }

        Status = TransactionStatus.Cancelled;
    }
}
