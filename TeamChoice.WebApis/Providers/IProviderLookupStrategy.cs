using TeamChoice.WebApis.Models.DTOs;

namespace TeamChoice.WebApis.Providers
{
    public interface IProviderLookupStrategy
    {
        bool Supports(string serviceCode);
        Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode);
    }
}
