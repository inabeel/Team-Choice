using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Domain.Models.DTOs;
using TeamChoice.WebApis.Infrastructure.Providers;

namespace TeamChoice.WebApis.Infrastructure.Providers.MMT;

public class MPesaMMTStrategy : MMTBaseClass, IProviderLookupStrategy
{
    public MPesaMMTStrategy(HttpClient httpClient, ServiceProviderProperties properties)
        : base(httpClient, properties)
    {
    }

    public bool Supports(string serviceCode)
    {
        return "00014" == serviceCode;
    }

    // This overloads the base LookupAsync method signature found in the interface
    public async Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode)
    {
        var replacedPhone = phoneNumber.Replace("+", "");

        return await base.LookupAsync(
            replacedPhone,
            serviceCode,
            "mpesa",
            "KEM",
            "KE"
        );
    }
}
