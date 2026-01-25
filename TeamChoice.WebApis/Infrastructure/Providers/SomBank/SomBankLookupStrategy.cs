using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Contracts;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Infrastructure.Providers.Security;

namespace TeamChoice.WebApis.Infrastructure.Providers.SomBank;

public class SomBankLookupStrategy : IProviderLookupStrategy
{
    private readonly HttpClient _httpClient;
    private readonly EncryptDecryptService _encryptDecryptService;
    private readonly ServiceProviderProperties _serviceProperties;

    public SomBankLookupStrategy(
        HttpClient httpClient,
        EncryptDecryptService encryptDecryptService,
        ServiceProviderProperties serviceProperties)
    {
        _httpClient = httpClient;
        _encryptDecryptService = encryptDecryptService;
        _serviceProperties = serviceProperties;
    }

    public bool Supports(string serviceCode)
    {
        return "00010".Equals(serviceCode);
    }

    public async Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode)
    {
        var replacedPhone = phoneNumber.Replace("+", "");
        var encryptedPhone = _encryptDecryptService.Encrypt(replacedPhone);

        // Ensure the URL is constructed correctly. 
        // In Java: serviceProperties.getSomBank().getLink()+"sombankingapi..."
        var baseUrl = _serviceProperties.SomBank.Link;
        if (!baseUrl.EndsWith("/")) baseUrl += "/";
        var url = $"{baseUrl}sombankingapi/sombankingapi/tawakalintegration/v1/api/getCustomerDetailsByPhone";

        var credentials = new SomBankCredentialsReq
        {
            ApiKey = _serviceProperties.SomBank.ApiKey,
            Username = _serviceProperties.SomBank.Username,
            Password = _serviceProperties.SomBank.Password
        };

        var criteria = new SomBankCriteriaReq
        {
            PhoneNo = encryptedPhone
        };

        var req = new SomBankLookupReq
        {
            Credentials = credentials,
            SelectionCriteria = criteria
        };

        try
        {
            var responseMsg = await _httpClient.PostAsJsonAsync(url, req);

            if (!responseMsg.IsSuccessStatusCode)
            {
                // You might want to log the error body here
                throw new Exception($"Error calling SomBank API: {responseMsg.StatusCode}");
            }

            var apiResponse = await responseMsg.Content.ReadFromJsonAsync<SomBankApiResponse>();

            // Handle customer list null/empty check
            if (apiResponse?.CustomerDetailsList == null || !apiResponse.CustomerDetailsList.Any())
            {
                throw new ArgumentException($"This service is not available for this phone number: {phoneNumber}");
            }

            // Extract Customer
            // Java code used .next() which implies the first element, despite the method name "extractSecondCustomer"
            var customer = apiResponse.CustomerDetailsList.FirstOrDefault();

            if (customer == null)
            {
                throw new ArgumentException($"This service is not available for this phone number: {phoneNumber}");
            }

            // Handle account nullability
            if (customer.AccountDetailsList == null || !customer.AccountDetailsList.Any())
            {
                throw new ArgumentException($"This service is not available for this phone number: {phoneNumber}");
            }

            // Extract First Account
            var account = customer.AccountDetailsList.FirstOrDefault();

            if (account == null)
            {
                throw new ArgumentException($"This service is not available for this phone number: {phoneNumber}");
            }

            return MapToResponse(customer, account, serviceCode, replacedPhone);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error contacting SomBank service", ex);
        }
    }

    private ServiceLookupResponse MapToResponse(
        SomBankCustomerDetail customer,
        SomBankAccountDetail acc,
        string serviceCode,
        string phoneNumber)
    {
        return new ServiceLookupResponse
        {
            Response = "success",
            ServiceCode = serviceCode,
            PhoneNumber = phoneNumber,
            AccountName = _encryptDecryptService.Decrypt(customer.ShortName),
            AccountId = _encryptDecryptService.Decrypt(acc.AccountNo),
            Provider = "sombank",
            CountryCode = "SO",
            CurrencyCode = "USD"
        };
    }
}
