using TeamChoice.WebApis.Contracts;
using TeamChoice.WebApis.Contracts.DTOs;

namespace TeamChoice.WebApis.Infrastructure.Providers.MMT;

public class MMTBaseClass
{
    private readonly HttpClient _httpClient;
    private readonly ServiceProviderProperties _serviceProperties;

    public MMTBaseClass(HttpClient httpClient, ServiceProviderProperties serviceProperties)
    {
        _httpClient = httpClient;
        _serviceProperties = serviceProperties;
    }

    public virtual async Task<ServiceLookupResponse> LookupAsync(
        string phoneNumber,
        string serviceCode,
        string serviceType,
        string agentCode,
        string countryCode)
    {
        var replacedPhone = phoneNumber.Replace("+", "");
        // Assuming the link property ends with / or the API path handles it. 
        // In C# Url combination is safer, but strictly following the string concat logic here:
        var url = $"{_serviceProperties.Agent.Link}tawakalAPI/api/v1/Tawakal/validateaccount";

        var req = new MMTLookupReq
        {
            AccountNo = replacedPhone,
            AccountAgentCode = agentCode,
            Location = _serviceProperties.Agent.Location,
            Username = _serviceProperties.Agent.Username,
            Password = _serviceProperties.Agent.Password,
            AgentCode = _serviceProperties.Agent.AgentCode
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, req);

            // Ensure we got a valid HTTP response before trying to parse
            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<MMTApiResponse>();

            if (apiResponse?.Validate == null || !apiResponse.Validate.Any())
            {
                throw new ArgumentException($"This service is not available for this phone number: +{phoneNumber}");
            }

            return MapToMMTResponse(
                apiResponse.Validate.First(),
                serviceCode,
                phoneNumber,
                serviceType,
                countryCode
            );
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP specific errors or rethrow
            throw new Exception("Error contacting external service provider", ex);
        }
    }

    private ServiceLookupResponse MapToMMTResponse(
        MMTAccount acc,
        string serviceCode,
        string phoneNumber,
        string serviceType,
        string countryCode)
    {
        return new ServiceLookupResponse
        {
            Response = "success",
            ServiceCode = serviceCode,
            PhoneNumber = phoneNumber,
            AccountName = acc.AccountName,
            AccountId = phoneNumber,
            Provider = serviceType,
            CountryCode = countryCode,
            CurrencyCode = "USD"
        };
    }
}