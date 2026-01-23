namespace TeamChoice.WebApis.Domain.Exceptions;

public sealed class TransactionValidationException : Exception
{
    public TransactionValidationException(string message) : base(message) { }
}

public sealed class InvalidTransactionStatusException : Exception
{
    public InvalidTransactionStatusException(string message) : base(message) { }
}

public sealed class AlreadyPaidException : Exception
{
    public AlreadyPaidException(string message) : base(message) { }
}

public sealed class TransactionForwardingException : Exception
{
    public TransactionForwardingException(string message) : base(message) { }
}

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}