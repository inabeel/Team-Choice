using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Controllers;

[ApiController]
[Route("api/v1/transaction")]
//[Authorize]
public class TransactionsController : ControllerBase
{
    [HttpPost]
    public ActionResult<HttpResponseDto<TransactionResultDto>> CreateTransaction(
        [FromBody] TransactionRequestDto request)
    {
        // TODO: transaction processing
        var result = new TransactionResultDto
        {
            Status = "Pending",
            TawakalTxnRef = "TWK-REF-001234"
        };

        return Ok(new HttpResponseDto<TransactionResultDto>
        {
            StatusCode = 200,
            Status = "OK",
            Data = result
        });
    }

    [HttpPost("status")]
    public ActionResult<HttpResponseDto<TransactionResultDto>> ValidateTransactionStatus(
        [FromBody] TransactionStatusRequestDto request)
    {
        // TODO: status validation
        return Ok(new HttpResponseDto<TransactionResultDto>
        {
            StatusCode = 200,
            Status = "OK",
            Data = new TransactionResultDto
            {
                Status = "Completed",
                TawakalTxnRef = request.TransactionReference
            }
        });
    }

    [HttpPost("cancel")]
    public ActionResult<HttpResponseDto<object>> CancelTransaction(
        [FromBody] CancelTransactionRequestDto request)
    {
        // TODO: cancellation logic
        return Ok(new HttpResponseDto<object>
        {
            StatusCode = 200,
            Status = "OK",
            Message = "Transaction cancelled successfully"
        });
    }
}