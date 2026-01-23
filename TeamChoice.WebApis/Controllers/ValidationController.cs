using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamChoice.WebApis.Models;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Models.DTOs.Transactions;
using System.Net; // Added to likely resolve ApiResponse if moved there

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/v1/transaction/status")]
    public class ValidationController : ControllerBase
    {
        private readonly IAgentTransactionFacade _agentTransactionFacade;
        private readonly ILogger<ValidationController> _logger;

        public ValidationController(IAgentTransactionFacade agentTransactionFacade, ILogger<ValidationController> logger)
        {
            _agentTransactionFacade = agentTransactionFacade;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateTransactionStatus([FromBody] TransactionStatusRequestDto request)
        {
            _logger.LogInformation("🔍 Validating status for transactionReference: {Ref}", request.TawakalTxnRef);

            try
            {
                var status = await _agentTransactionFacade.ValidateTransactionStatusAsync(request.TawakalTxnRef);
                return Respond((int)HttpStatusCode.OK, "Transaction status retrieved successfully", status);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "❌ Failed to validate transaction status for {Ref}: {Message}", request.TawakalTxnRef, error.Message);
                // Depending on requirements, might want to return 500 or 400 here
                return StatusCode(500); 
            }
        }

        private IActionResult Respond(int statusCode, string message, string txnStatus)
        {
            var response = new ApiResponse
            {
                TimeStamp = DateTime.Now.ToString(),
                Status = statusCode == 200 ? "OK" : "ERROR",
                StatusCode = statusCode,
                Data = new TransactionResult
                {
                    Status = txnStatus,
                    Message = message
                }
            };
            return StatusCode(statusCode, response);
        }
    }
}