using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Controllers;

[ApiController]
[Route("api/v1/rate")]
[Authorize]
public class ExchangeRateController : Controller
{
    [HttpPost("exchange-rate")]
    public ActionResult<HttpResponseDto<ExchangeResponseBuilderDto>> GetExternalExchangeRate(
        [FromBody] ExchangePayloadDto request)
    {
        // TODO: call service layer
        var response = new ExchangeResponseBuilderDto
        {
            Timestamp = DateTime.UtcNow,
            StatusCode = 200,
            StatusMessage = "SUCCESS"
        };

        return Ok(new HttpResponseDto<ExchangeResponseBuilderDto>
        {
            StatusCode = 200,
            Status = "OK",
            Data = response
        });
    }
}