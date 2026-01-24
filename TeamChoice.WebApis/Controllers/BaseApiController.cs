using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Controllers;

[Authorize]
[ApiController]
[Produces("application/json")]

[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult OkResponse<T>(T data, string? message = null)
    {
        return Ok(new HttpResponseDto<T>
        {
            TimeStamp = DateTime.UtcNow,
            StatusCode = StatusCodes.Status200OK,
            Status = "OK",
            Message = message,
            Data = data
        });
    }
}
