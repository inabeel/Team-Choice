using TeamChoice.WebApis.Domain.Models;

namespace TeamChoice.WebApis.Infrastructure.Providers;

public interface IProviderLookupStrategy
{
    bool Supports(string serviceCode);

    Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode);
}
