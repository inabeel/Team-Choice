using System.Text.RegularExpressions;
using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Contracts.DTOs.Transactions;

namespace TeamChoice.WebApis.Application.Services.Transactions;

public interface IServiceTypeResolver
{
    Task<string> ResolveAsync(TransactionRequestDto request);
}

public sealed class ServiceTypeResolver : IServiceTypeResolver
{
    private readonly IAgentTransactionFacade _agentTransactionFacade;

    public ServiceTypeResolver(
        IAgentTransactionFacade agentTransactionFacade)
    {
        _agentTransactionFacade = agentTransactionFacade;
    }

    public async Task<string> ResolveAsync(
        TransactionRequestDto request)
    {
        var serviceCode = request.Payment.ServiceCode?.Trim() ?? "";

        if (Regex.IsMatch(serviceCode, @"^\d{6}$"))
        {
            var resolved =
                await _agentTransactionFacade
                    .FindServiceCodeUsingBankServiceTypeAsync(serviceCode);

            return resolved
                ?? throw new InvalidServiceCodeException(serviceCode);
        }

        // fire-and-forget logic removed from core flow
        return serviceCode;
    }
}
