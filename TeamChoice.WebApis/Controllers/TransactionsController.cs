using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TeamChoice.WebApis.Application.Facades;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.DTOs.Transactions;

namespace TeamChoice.WebApis.Controllers;

[Route("api/v1/transaction")]
public class TransactionsController : BaseApiController
{
    private readonly ITransactionFacade _facade;

    public TransactionsController(ITransactionFacade facade)
    {
        _facade = facade;
    }

    [SwaggerOperation(
    Summary = "Create transaction",
    Description = "Accepts a transaction request and initiates processing. Returns a pending reference when accepted.",
    OperationId = "createTransaction",
    Tags = new[] { "transactions" })]

    [HttpPost]
    [ProducesResponseType(typeof(HttpResponseDto<TransactionResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTransactionAsync([FromBody] TransactionRequestDto request)
    {
        var result = await _facade.CreateAsync(request);

        return OkResponse(result);
    }

    [HttpPost("status")]
    [ProducesResponseType(typeof(HttpResponseDto<TransactionResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateTransactionStatusAsync([FromBody] TransactionStatusRequestDto request)
    {
        var status = await _facade.ValidateStatusAsync(request);

        return OkResponse(status);
    }

    [HttpPost("cancel")]
    [SwaggerOperation(
        Summary = "Cancel transaction",
        Description = "Attempts to cancel a transaction if it is still in a cancellable state",
        OperationId = "cancelTransaction",
        Tags = new[] { "transactions" }
    )]

    [ProducesResponseType(typeof(HttpResponseDto<TransactionResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel([FromBody] CancelTransactionRequest request)
    {
        var result = await _facade.CancelAsync(request);

        return OkResponse(result);
    }
}