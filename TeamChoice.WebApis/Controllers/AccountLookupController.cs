using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Models;
using TeamChoice.WebApis.Models.DTOs.Transactions; // For ApiResponse if located here


namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/v1/transaction")]
    public class AccountLookupController : ControllerBase
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

            // Using standard Ok() with ApiResponse structure directly for simplicity, 
            // or could use a helper method if strictly following CancelRequestController pattern.
            var response = new ApiResponse
            {
                TimeStamp = DateTime.Now.ToString(),
                Status = "OK",
                StatusCode = 200,
                Data = data
            };

            return Ok(response);
        }

        [HttpPost("account-lookup")]
        public async Task<IActionResult> LookupAccount([FromBody] AccountsLookupRequest request)
        {
            _logger.LogInformation("📥 Received account lookup request: {@Request}", request);

            if (string.IsNullOrWhiteSpace(request.ServiceCode) || !"231404".Equals(request.ServiceCode, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("❌ Invalid service code: {Code}", request.ServiceCode);
                return BadRequest(new ApiResponse
                {
                    TimeStamp = DateTime.Now.ToString(),
                    Status = "BAD_REQUEST",
                    StatusCode = 400,
                    Data = new Dictionary<string, string> { { "error", "Invalid service code" } }
                });
            }

            if (string.IsNullOrWhiteSpace(request.ServiceMode) || !"1000594563248".Equals(request.ServiceMode, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("❌ Invalid account number: {Code}", request.ServiceCode);
                return BadRequest(new ApiResponse
                {
                    TimeStamp = DateTime.Now.ToString(),
                    Status = "BAD_REQUEST",
                    StatusCode = 400,
                    Data = new Dictionary<string, string> { { "error", "Invalid account number" } }
                });
            }

            try
            {
                var resp = await _accountLookupService.LookupAccountAsync(request);
                _logger.LogInformation("✅ Account lookup response: {@Resp}", resp);

                return Ok(new ApiResponse
                {
                    TimeStamp = DateTime.Now.ToString(),
                    Data = resp,
                    Status = "OK",
                    StatusCode = 200,
                    Message = "Account lookup successful"
                });
            }
            catch (Exception error)
            {
                _logger.LogError("❌ Error during account lookup: {Message}", error.Message);
                return StatusCode(500, new ApiResponse
                {
                    TimeStamp = DateTime.Now.ToString(),
                    Status = "INTERNAL_SERVER_ERROR",
                    StatusCode = 500,
                    Message = "Account lookup failed: " + error.Message
                });
            }
        }
    }
}