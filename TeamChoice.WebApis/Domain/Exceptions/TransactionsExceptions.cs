namespace TeamChoice.WebApis.Domain.Exceptions;


/// <summary>
/// Base exception for all domain-level transaction errors.
/// Application and middleware should depend on this type.
/// </summary>
public abstract class TransactionDomainException : Exception
{
    protected TransactionDomainException(string message) : base(message)
    {
    }

    protected TransactionDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

#region Validation Exceptions

/// <summary>
/// Thrown when a transaction request fails domain validation rules.
/// </summary>
public sealed class TransactionValidationException : TransactionDomainException
{
    public TransactionValidationException(string message) : base(message)
    {
    }
}

/// <summary>
/// Thrown when an invalid or unsupported transaction status is encountered.
/// </summary>
public sealed class InvalidTransactionStatusException : TransactionDomainException
{
    public InvalidTransactionStatusException(string status) : base($"Invalid transaction status: {status}")
    {
    }
}

#endregion

#region State / Conflict Exceptions

/// <summary>
/// Thrown when an operation is attempted on a transaction
/// that is already completed or paid.
/// </summary>
public sealed class AlreadyPaidException : TransactionDomainException
{
    public AlreadyPaidException(string transactionReference) : base($"Transaction already paid: {transactionReference}")
    {
    }
}

/// <summary>
/// Thrown when a duplicate transaction reference is detected.
/// </summary>
public sealed class DuplicateTransactionException : TransactionDomainException
{
    public DuplicateTransactionException(string transactionReference) : base($"Duplicate transaction reference: {transactionReference}")
    {
    }
}

#endregion

#region Not Found Exceptions

/// <summary>
/// Base exception for entity-not-found scenarios.
/// </summary>
public abstract class NotFoundException : TransactionDomainException
{
    protected NotFoundException(string message) : base(message)
    {
    }
}

/// <summary>
/// Thrown when a transaction reference cannot be found.
/// </summary>
public sealed class TransactionNotFoundException : NotFoundException
{
    public TransactionNotFoundException(string transactionReference) : base($"Transaction not found: {transactionReference}")
    {
    }
}

/// <summary>
/// Thrown when a required service or service code cannot be resolved.
/// </summary>
public sealed class ServiceNotFoundException : NotFoundException
{
    public ServiceNotFoundException(string serviceCode) : base($"Service not found for code: {serviceCode}")
    {
    }
}

#endregion

#region External / Integration Exceptions

/// <summary>
/// Thrown when forwarding a transaction to an external system fails.
/// </summary>
public sealed class TransactionForwardingException : TransactionDomainException
{
    public string TransactionReference { get; }

    public TransactionForwardingException(string transactionReference, string message) : base(message)
    {
        TransactionReference = transactionReference;
    }

    public TransactionForwardingException(string transactionReference, Exception innerException)
        : base($"Transaction forwarding failed for reference '{transactionReference}'.", innerException)
    {
        TransactionReference = transactionReference;
    }

    public TransactionForwardingException(
        string transactionReference,
        string message,
        Exception innerException)
        : base(message, innerException)
    {
        TransactionReference = transactionReference;
    }
}

/// <summary>
/// Thrown when a remittance provider returns an invalid or failed response.
/// </summary>
public sealed class RemittanceFailedException
    : TransactionDomainException
{
    public RemittanceFailedException(string message)
        : base(message)
    {
    }
}

#endregion
