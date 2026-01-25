using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TeamChoice.WebApis.Application.Orchestrators;
using TeamChoice.WebApis.Domain.Models.DTOs;
using TeamChoice.WebApis.Domain.Models.DTOs.Exchanges;

namespace TeamChoice.WebApis.Controllers;

[Route("api/v1/rate")]
[SwaggerTag("Exchange rate endpoints")]
public class ExchangeRateController : BaseApiController
{
    private readonly IRateOrchestrator _rateOrchestrator;

    public ExchangeRateController(IRateOrchestrator rateOrchestrator)
    {
        _rateOrchestrator = rateOrchestrator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Get internal exchange rate",
        Description = "Computes and returns internal exchange rate details for a given payload",
        OperationId = "getInternalExchangeRate",
        Tags = new[] { "transactions" })]

    [ProducesResponseType(typeof(HttpResponseDto<ExchangeRateResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExchangeRate([FromBody] ExchangeRatePayloadDto payload)
    {
        var result = await _rateOrchestrator.GetInternalRateAsync(payload);
        return OkResponse(result);
    }

    [HttpPost("exchange-rate")]
    [SwaggerOperation(
         Summary = "Get external partner exchange rate",
         Description = "Returns computed exchange rate and fees for an external partner request",
         OperationId = "getExternalExchangeRate",
         Tags = new[] { "transactions" })]

    [ProducesResponseType(typeof(HttpResponseDto<ExchangeResponseBuilderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExternalExchangeRate([FromBody] ExchangeRatePayloadDto payload)
    {
        var result = await _rateOrchestrator.GetExternalRateAsync(payload);
        return OkResponse(result);
    }
}