using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Contracts;
using TeamChoice.WebApis.Contracts.DTOs;

namespace TeamChoice.WebApis.Infrastructure.Providers.TPlus;

public class TplusLookupStrategy : IProviderLookupStrategy
{
    private readonly HttpClient _httpClient;
    private readonly ServiceProviderProperties _serviceProperties;

    public TplusLookupStrategy(HttpClient httpClient, ServiceProviderProperties serviceProperties)
    {
        _httpClient = httpClient;
        _serviceProperties = serviceProperties;
    }

    public bool Supports(string serviceCode)
    {
        return "00003".Equals(serviceCode);
    }

    public async Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode)
    {
        var replacedPhoneNumber = phoneNumber.Replace("+", "");

        // Construct URL: properties.getTPlus().getLink() + "api/v1/tplus/ValidateTPlusAccount"
        var baseUrl = _serviceProperties.TPlus.Link;
        if (!baseUrl.EndsWith("/")) baseUrl += "/";
        var url = $"{baseUrl}api/v1/tplus/ValidateTPlusAccount";

        var username = _serviceProperties.TPlus.Username;
        var password = _serviceProperties.TPlus.Password;

        var req = new TplusLookupReq
        {
            Account = replacedPhoneNumber,
            Username = username,
            Password = password
        };

        try
        {
            var responseMsg = await _httpClient.PostAsJsonAsync(url, req);

            if (!responseMsg.IsSuccessStatusCode)
            {
                throw new Exception($"Error calling TPlus API: {responseMsg.StatusCode}");
            }

            var apiResponse = await responseMsg.Content.ReadFromJsonAsync<TplusApiResponse>();

            if (apiResponse == null)
            {
                throw new ArgumentException($"This service is not available for this phone number: {phoneNumber}");
            }

            // Logic: findPersonalAccount -> filter(acc => "Personal".equalsIgnoreCase(type)).next()
            var personalAccount = FindPersonalAccount(apiResponse);

            if (personalAccount == null)
            {
                throw new ArgumentException($"This service is not available for this phone number: {phoneNumber}");
            }

            return MapToResponse(personalAccount, serviceCode, phoneNumber);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error contacting TPlus service", ex);
        }
    }

    private TplusAccount FindPersonalAccount(TplusApiResponse response)
    {
        if (response.Accounts == null) return null;

        return response.Accounts
            .FirstOrDefault(acc => "Personal".Equals(acc.AccountType, StringComparison.OrdinalIgnoreCase));
    }

    private ServiceLookupResponse MapToResponse(
        TplusAccount acc,
        string serviceCode,
        string phoneNumber)
    {
        return new ServiceLookupResponse
        {
            Response = "success",
            ServiceCode = serviceCode,
            PhoneNumber = phoneNumber,
            AccountName = acc.AccountName,
            AccountId = acc.AccountNo,
            Provider = "tplus",
            CountryCode = "SO",
            CurrencyCode = acc.Currency
        };
    }
}
