using TeamChoice.WebApis.Models.DTOs.Services;

namespace TeamChoice.WebApis.Domain.Services;

public interface ITplusLookupService
{
    Task<ServiceLookupResponseDto> LookupAsync(AccountsLookupRequestDto request);
}

/// <summary>
/// Handles T+ service lookups and provider mapping.
/// </summary>
public sealed class TplusLookupService : ITplusLookupService
{
    private readonly HttpClient _httpClient; // kept for parity / future use

    public TplusLookupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ServiceLookupResponseDto> LookupAsync(
        AccountsLookupRequestDto request)
    {
        var serviceCode = request.ServiceCode?.Trim();

        var provider = serviceCode switch
        {
            "00003" => "tplus",
            "00010" => "sombank",
            "00014" => "mpesa",
            "00006" => "mmt",
            null => "unknown",
            _ => "none"
        };

        var phoneNumber =
            serviceCode is "00006" or "00014"
            && !string.IsNullOrWhiteSpace(request.PaymentMode)
                ? $"{request.PhoneNumber?.Trim()}|{request.PaymentMode.Trim()}"
                : request.PhoneNumber;

        var response = new ServiceLookupResponseDto
        {
            Success = true,
            Message = "success",
            Services =
            [
                new ServiceDetailDto
                {
                    ServiceCode = serviceCode,
                    ServiceName = "John Doe ",
                    ProviderName = provider,
                    CountryCode = "SO",
                    CurrencyCode = "USD",
                    Description = "Lookup result",
                    Active = true
                }
            ]
        };

        return Task.FromResult(response);
    }
}
