using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Contracts.DTOs;

namespace TeamChoice.WebApis.Controllers;

[ApiController]
[Route("api/v1/services")]
public class ServiceLookupController : BaseApiController
{
    private readonly ILocServicesService _locServicesService;
    private readonly string _locId;

    public ServiceLookupController(ILocServicesService locServicesService, IConfiguration configuration)
    {
        _locServicesService = locServicesService;
        _locId = configuration["serviceLodId"]; // Matches @Value("${serviceLodId}")
    }

    [HttpGet]
    public async Task<IEnumerable<LocServiceRes>> GetAvailableServices()
    {
        return await _locServicesService.GetByLocIdAsync(_locId);
    }
}
