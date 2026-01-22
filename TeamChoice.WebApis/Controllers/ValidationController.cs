using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TeamChoice.WebApis.Models.DTOs.Transactions;
using TeamChoice.WebApis.Services;

namespace TeamChoice.WebApis.Controllers;

[ApiController]
[Route("api/v1/transaction/status")]
[Authorize]
public class ValidationController : Controller
{
    private readonly IAgentTransactionFacade _agentTransactionFacade;
    private readonly ILogger<ValidationController> _logger;

    public ValidationController(
        IAgentTransactionFacade agentTransactionFacade,
        ILogger<ValidationController> logger)
    {
        _agentTransactionFacade = agentTransactionFacade;
        _logger = logger;
    }

    /// <summary>
    /// Validates the status of a transaction based on the transaction reference.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(HttpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<HttpResponse>> ValidateTransactionStatus(
        [FromBody] TransactionStatusRequestDto request)
    {
        _logger.LogInformation(
            "Validating status for transactionReference: {TransactionReference}",
            request.TransactionReference);

        try
        {
            var status = await _agentTransactionFacade
                    .ValidateTransactionStatusAsync(request.TransactionReference);

            return Ok("Transaction status retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to validate transaction status for {TransactionReference}",
                request.TransactionReference);

            throw; // handled by global exception middleware
        }
    }
}