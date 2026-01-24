using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Application.Facades;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Controllers;

[ApiController]
[Route("api/v1/transaction/status")]
[Authorize]
public sealed class ValidationController : ControllerBase
{
    private readonly ITransactionValidationFacade _facade;

    public ValidationController(ITransactionValidationFacade facade)
    {
        _facade = facade;
    }

    [HttpPost]
    public async Task<ActionResult<HttpResponse>> ValidateTransactionStatus(
        [FromBody] TransactionStatusRequestDto request)
    {
        var response = await _facade.ValidateAsync(request);
        return Ok(response);
    }
}