using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Application.Services
{
    public interface IAccountLookupService
    {
        Task<AccountLookupResponse> LookupAccountAsync(AccountsLookupRequest request);
    }
    public class AccountLookupService : IAccountLookupService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AccountLookupService> _logger;

        private readonly string _accountLookupUrl;
        private readonly string _username;
        private readonly string _password;

        public AccountLookupService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<AccountLookupService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Mapping @Value("${...}") to Configuration
            _accountLookupUrl = configuration["account:lookup:url"];
            _username = configuration["account:lookup:username"];
            _password = configuration["account:lookup:password"];
        }

        public async Task<AccountLookupResponse> LookupAccountAsync(AccountsLookupRequest request)
        {
            // --- Active Mock Implementation (Matches Java) ---
            var response = new AccountLookupResponse
            {
                Response = "success",
                ServiceType = "Bank Service",
                ServiceCode = "231404",
                ServiceName = "Awash Bank",
                ServiceMode = "1000607425338",
                AccountName = "Giftiwot soma mohammed",
                CountryCode = "ET",
                CurrencyCode = "ETB"
            };

            return await Task.FromResult(response);

            // --- Real Implementation (Converted from commented Java code) ---
            /*
            var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));

            _logger.LogInformation("🌐 Performing account lookup to {Url}", _accountLookupUrl);

            try 
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, _accountLookupUrl);
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", authString);
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpRequest.Content = JsonContent.Create(request);

                var httpResponse = await _httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();

                var lookupResponse = await httpResponse.Content.ReadFromJsonAsync<AccountLookupResponse>();

                _logger.LogInformation("✅ Account lookup successful: {@Response}", lookupResponse);
                return lookupResponse;
            }
            catch (Exception error)
            {
                _logger.LogError(error, "❌ Account lookup failed: {Message}", error.Message);
                _logger.LogError("⚠️ Exception during account lookup: {Message}", error.Message);
                throw new Exception("Failed to lookup account", error);
            }
            */
        }
    }
}
