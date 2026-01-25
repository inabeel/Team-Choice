using TeamChoice.WebApis.Domain.Models;

namespace TeamChoice.WebApis.Application.Interfaces.Services;

public interface IProviderLookupStrategy
{
    bool Supports(string serviceCode);

    Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode);
}
