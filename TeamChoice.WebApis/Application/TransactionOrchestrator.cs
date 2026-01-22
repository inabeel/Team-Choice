using TeamChoice.WebApis.Domain.Processors;
using TeamChoice.WebApis.Domain.Validation;
using TeamChoice.WebApis.Models.DTOs.Transactions;

namespace TeamChoice.WebApis.Application;

public interface ITransactionOrchestrator
{
    Task<TransactionOrchestrationResult> ExecuteAsync(
        TransactionRequestDto request);
}

/// <summary>
/// Coordinates validation and processing of a transaction.
/// </summary>
public sealed class TransactionOrchestrator : ITransactionOrchestrator
{
    private readonly ITransactionValidator _validator;
    private readonly ITransactionProcessor _processor;
    private readonly ILogger<TransactionOrchestrator> _logger;

    public TransactionOrchestrator(
        ITransactionValidator validator,
        ITransactionProcessor processor,
        ILogger<TransactionOrchestrator> logger)
    {
        _validator = validator;
        _processor = processor;
        _logger = logger;
    }

    public async Task<TransactionOrchestrationResult> ExecuteAsync(
        TransactionRequestDto request)
    {
        _logger.LogInformation("📥 Starting transaction orchestration");

        // 1️⃣ Validate request (business rules)
        _validator.Validate(request);

        // 2️⃣ Process transaction
        var result = await _processor.ProcessAsync(request);

        _logger.LogInformation(
            "✅ Transaction processed successfully. Reference={Reference}",
            result.Reference);

        return result;
    }
}

public sealed record TransactionOrchestrationResult(
    string Reference);

