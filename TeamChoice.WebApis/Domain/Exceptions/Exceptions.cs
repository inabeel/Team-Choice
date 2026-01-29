using TeamChoice.WebApis.Domain.Exceptions;

/// <summary>
/// Thrown when a provided service code is syntactically valid
/// but not supported or allowed by business rules.
/// </summary>
public sealed class InvalidServiceCodeException : TransactionDomainException
{
    public InvalidServiceCodeException(string serviceCode) : base($"Invalid service code: {serviceCode}") { }
}