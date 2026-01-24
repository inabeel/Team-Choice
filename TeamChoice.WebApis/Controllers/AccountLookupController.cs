using Microsoft.AspNetCore.Mvc;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Controllers;

[Route("api/v1/transaction")]
public class AccountLookupController : BaseApiController
{
    private readonly IAccountLookupService _accountLookupService;
    private readonly ILookupService _mLookupService;
    private readonly ILogger<AccountLookupController> _logger;

    public AccountLookupController(
        IAccountLookupService accountLookupService,
        ILookupService mLookupService,
        ILogger<AccountLookupController> logger)
    {
        _accountLookupService = accountLookupService;
        _mLookupService = mLookupService;
        _logger = logger;
    }

    [HttpPost("tawakal-service-Lookup")]
    public async Task<IActionResult> Lookup([FromBody] AccountsLookupRequest request)
    {
        var data = await _mLookupService.LookupAsync(request.PhoneNumber, request.ServiceCode);

        return OkResponse(data);
    }

    [HttpPost("account-lookup")]
    public async Task<IActionResult> LookupAccount([FromBody] AccountsLookupRequest request)
    {
        _logger.LogInformation("📥 Received account lookup request: {@Request}", request);

        if (!IsValidServiceCode(request.ServiceCode))
        {
            return BadRequestResponse("Invalid service code");
        }

        if (!IsValidAccountNumber(request.ServiceMode))
        {
            return BadRequestResponse("Invalid account number");
        }

        try
        {
            var response = await _accountLookupService.LookupAccountAsync(request);

            _logger.LogInformation("✅ Account lookup successful");

            return OkResponse(response, "Account lookup successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Account lookup failed");

            return InternalServerErrorResponse(
                "Account lookup failed",
                ex.Message);
        }
    }

    // --------------------------------------------------------------------
    // Helpers
    // --------------------------------------------------------------------
    private static bool IsValidServiceCode(string? serviceCode) =>
        !string.IsNullOrWhiteSpace(serviceCode) &&
        serviceCode.Equals("231404", StringComparison.OrdinalIgnoreCase);

    private static bool IsValidAccountNumber(string? serviceMode) =>
        !string.IsNullOrWhiteSpace(serviceMode) &&
        serviceMode.Equals("1000594563248", StringComparison.OrdinalIgnoreCase);
}