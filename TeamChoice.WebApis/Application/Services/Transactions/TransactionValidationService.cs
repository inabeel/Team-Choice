using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Contracts.DTOs.Transactions;
using TeamChoice.WebApis.Domain.Exceptions;

namespace TeamChoice.WebApis.Application.Services.Transactions;

public interface ITransactionValidationService
{
    Task ValidateAsync(TransactionRequestDto request);
}

public sealed class TransactionValidationService : ITransactionValidationService
{
    private readonly IAgentTransactionFacade _agentTransactionFacade;

    public TransactionValidationService(IAgentTransactionFacade agentTransactionFacade)
    {
        _agentTransactionFacade = agentTransactionFacade;
    }

    public async Task ValidateAsync(TransactionRequestDto request)
    {
        if (request?.Sender?.PhoneNumber is null)
            throw new TransactionValidationException("Missing sender phone");

        if (!string.IsNullOrWhiteSpace(request.PartnerReference))
        {
            var result = await _agentTransactionFacade.ValidateTransactionAsync(request.PartnerReference);

            if (!string.IsNullOrWhiteSpace(result)
                && !"NOT_FOUND".Equals(result, StringComparison.OrdinalIgnoreCase))
            {
                throw new DuplicateTransactionException(request.PartnerReference);
            }
        }
    }
}
