using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Services;

namespace TeamChoice.WebApis.Controllers;

[ApiController]
[Route("api/v1/transaction/cancel")]
public class CancelRequestController : ControllerBase
{
    private readonly ICancellationService _cancellationService;
    private readonly IAgentTransactionFacade _agentTransactionFacade;
    private readonly ClientUrlProperties _clientProps;
    private readonly ILogger<CancelRequestController> _logger;

    public CancelRequestController(
        ICancellationService cancellationService,
        IAgentTransactionFacade agentTransactionFacade,
        ClientUrlProperties clientProps,
        ILogger<CancelRequestController> logger)
    {
        _cancellationService = cancellationService;
        _agentTransactionFacade = agentTransactionFacade;
        _clientProps = clientProps;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Cancel([FromBody] CancelTransactionDTO request)
    {
        _logger.LogInformation("🛑 Received cancellation request for transactionId={TransactionId}", request.TawakalTxnRef);

        var cancelPayload = BuildCancelRequest(request);

        try
        {
            var status = await _agentTransactionFacade.ValidateTransactionStatusAsync(request.TawakalTxnRef);

            if (string.IsNullOrEmpty(status))
            {
                // Handle "Transaction not found" case from switchIfEmpty logic
                throw new ArgumentException("Transaction not found: " + request.TawakalTxnRef);
            }

            _logger.LogInformation("✅ Transaction {TransactionId} current status: {Status}", request.TawakalTxnRef, status);

            if (!IsCancellable(status))
            {
                return Respond(HttpStatus.OK, "Transaction is not valid for cancellation", "error", request.TawakalTxnRef);
            }

            var resultDTO = await _cancellationService.CancelTransactionAsync(cancelPayload);

            if (resultDTO != null)
            {
                return Respond(HttpStatus.OK, "Transaction successfully cancelled", "Cancelled", request.TawakalTxnRef);
            }
            else
            {
                return Respond(HttpStatus.BadRequest, "Cancellation failed — provider did not respond", "error", request.TawakalTxnRef);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "❌ Error during cancellation process: {Message}", e.Message);
            return Respond(HttpStatus.InternalServerError, "Unexpected error occurred during cancellation", "FAILED", request.TawakalTxnRef);
        }
    }

    private bool IsCancellable(string status)
    {
        return "READY".Equals(status, StringComparison.OrdinalIgnoreCase);
    }

    private CancelReceiveRequest BuildCancelRequest(CancelTransactionDTO dto)
    {
        // Assuming Mapper exists or manual mapping
        var req = new CancelReceiveRequest
        {
            // Map fields from dto...
            TawakalTxnRef = dto.TawakalTxnRef
        };
        req.Loccode = _clientProps.Loccode;
        req.Agtaprvduser = _clientProps.UserCred;
        return req;
    }

    private IActionResult Respond(int statusCode, string message, string txnStatus, string transactionId)
    {
        var response = new HttpResponse
        {
            TimeStamp = DateTime.Now.ToString(),
            Status = statusCode == 200 ? "OK" : "ERROR", // Simplified mapping
            StatusCode = statusCode,
            Data = new TransactionResult
            {
                Status = txnStatus,
                Message = message,
                TawakalTxnRef = transactionId
            }
        };
        return StatusCode(statusCode, response);
    }
}
