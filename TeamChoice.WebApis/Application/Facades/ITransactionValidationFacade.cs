using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Application.Facades;

public interface ITransactionValidationFacade
{
    Task<TransactionResultDto> ValidateAsync(TransactionStatusRequestDto request);
}

public sealed class TransactionValidationFacade : ITransactionValidationFacade
{
    private readonly IAgentTransactionFacade _agentTransactionFacade;
    private readonly ILogger<TransactionValidationFacade> _logger;

    public TransactionValidationFacade(
        IAgentTransactionFacade agentTransactionFacade,
        ILogger<TransactionValidationFacade> logger)
    {
        _agentTransactionFacade = agentTransactionFacade;
        _logger = logger;
    }

    public async Task<TransactionResultDto> ValidateAsync(TransactionStatusRequestDto request)
    {
        _logger.LogInformation(
            "Validating transaction status for {TransactionReference}",
            request.TransactionReference);

        var status =
            await _agentTransactionFacade
                .ValidateTransactionStatusAsync(request.TransactionReference);

        var response = new TransactionResultDto
        {
            Status = status,
            Message = new Dictionary<string, string>
            {
                ["info"] = "Transaction status retrieved successfully"
            }
        };

        return response;
    }
}
