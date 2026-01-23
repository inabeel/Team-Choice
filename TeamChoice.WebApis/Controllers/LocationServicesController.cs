using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Models.DTOs;

namespace TeamChoice.WebApis.Controllers
{
    [ApiController]
    [Route("api/v1/location-services/")]
    public class LocationServicesController : ControllerBase
    {
        private readonly ILocServicesService _locServicesService;

        public LocationServicesController(ILocServicesService locServicesService)
        {
            _locServicesService = locServicesService;
        }

        [HttpGet("{locId}")]
        public async Task<IEnumerable<LocServiceRes>> GetAvailableServices([FromRoute] string locId)
        {
            // Assuming service returns Task<IEnumerable<T>> or similar
            return await _locServicesService.GetByLocIdAsync(locId);
        }
    }
}
