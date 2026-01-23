using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Application.Services;

public interface ITplusLookupService
{
    Task<ServiceLookupResponse> LookupAsync(AccountsLookupRequest request);
}

public sealed class TplusLookupService : ITplusLookupService
{
    private readonly HttpClient _httpClient; // kept for parity if later needed

    public TplusLookupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ServiceLookupResponse> LookupAsync(AccountsLookupRequest request)
    {
        // Map provider by service code as requested
        var serviceCode = request.ServiceCode?.Trim();

        string provider = serviceCode switch
        {
            null => "unknown",
            "00003" => "tplus",
            "00010" => "sombank",
            "00014" => "mpesa",
            "00006" => "mmt",
            _ => "none"
        };

        bool requiresPaymentMode =
            serviceCode == "00006" || serviceCode == "00014";

        bool hasPaymentMode =
            !string.IsNullOrWhiteSpace(request.PaymentMode);

        var phoneNumber =
            serviceCode != null && requiresPaymentMode && hasPaymentMode
                ? $"{request.PhoneNumber?.Trim()}|{request.PaymentMode!.Trim()}"
                : request.PhoneNumber;

        var accountId =
            serviceCode != null && requiresPaymentMode
                ? request.PhoneNumber
                : "1-1060-49145-1-0";

        var response = new ServiceLookupResponse
        {
            Response = "success",
            ServiceCode = serviceCode,
            PhoneNumber = phoneNumber,
            AccountName = "John Doe ",
            AccountId = accountId,
            Provider = provider,
            CountryCode = "SO",
            CurrencyCode = "USD"
        };

        return Task.FromResult(response);
    }
}