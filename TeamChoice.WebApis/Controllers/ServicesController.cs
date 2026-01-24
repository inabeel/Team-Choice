using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Controllers;

[ApiController]
[Route("api/v1")]
[Authorize]
public class ServicesController : Controller
{
    //[HttpGet("services")]
    //public ActionResult<HttpResponseDto<IEnumerable<ServiceDetailDto>>> ListServices()
    //{
    //    // TODO: fetch services from upstream provider
    //    return Ok(new HttpResponseDto<IEnumerable<ServiceDetailDto>>
    //    {
    //        StatusCode = 200,
    //        Status = "OK",
    //        Data = Enumerable.Empty<ServiceDetailDto>()
    //    });
    //}

    //[HttpPost("transaction/tawakal-service-Lookup")]
    //public ActionResult<HttpResponseDto<AccountLookupResponseDto>> TplusServiceLookup(
    //    [FromBody] AccountsLookupRequestDto request)
    //{
    //    // TODO: service lookup logic
    //    return Ok(new HttpResponseDto<AccountLookupResponseDto>
    //    {
    //        StatusCode = 200,
    //        Status = "OK",
    //        Data = new AccountLookupResponseDto()
    //    });
    //}
}
