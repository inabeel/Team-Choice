using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TeamChoice.WebApis.Application.Facades;
using TeamChoice.WebApis.Application.Orchestrators;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Controllers;

[Route("api/v1/transaction")]
public class TransactionsController : BaseApiController
{
    private readonly ITransactionOrchestrator _transactionOrchestrator;
    private readonly ICancellationOrchestrator _cancellationOrchestrator;
    private readonly ITransactionValidationFacade _transactionValidationFacade;

    public TransactionsController(
        ITransactionOrchestrator transactionOrchestrator, 
        ICancellationOrchestrator cancellationOrchestrator,
        ITransactionValidationFacade transactionValidationFacade)
    {
        _transactionOrchestrator = transactionOrchestrator;
        _cancellationOrchestrator = cancellationOrchestrator;
        _transactionValidationFacade = transactionValidationFacade;
    }

    [SwaggerOperation(
    Summary = "Create transaction",
    Description = "Accepts a transaction request and initiates processing. Returns a pending reference when accepted.",
    OperationId = "createTransaction",
    Tags = new[] { "transactions" })]

    [HttpPost]
    [ProducesResponseType(typeof(HttpResponseDto<TransactionResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTransactionAsync([FromBody] TransactionRequestDTO request)
    {
        // 1️⃣ Call orchestrator (validation + processing happen inside)
        var orchestrationResult =
            await _transactionOrchestrator.HandleAsync(request);

        // TODO: transaction processing
        var result = new TransactionResultDto
        {
            Status = "Pending",
            TawakalTxnRef = orchestrationResult.Reference
        };

        return OkResponse(result);
    }

    [HttpPost("status")]
    [ProducesResponseType(typeof(HttpResponseDto<TransactionResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateTransactionStatusAsync([FromBody] TransactionStatusRequestDto request)
    {
        var response = await _transactionValidationFacade.ValidateAsync(request);
        return OkResponse(response);
    }

    [HttpPost("cancel")]
    [SwaggerOperation(
        Summary = "Cancel transaction",
        Description = "Attempts to cancel a transaction if it is still in a cancellable state",
        OperationId = "cancelTransaction",
        Tags = new[] { "transactions" }
    )]

    [ProducesResponseType(typeof(HttpResponseDto<TransactionResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel([FromBody] CancelTransactionDto request)
    {
        var result = await _cancellationOrchestrator.ExecuteAsync(request);

        return OkResponse(result);
    }
}