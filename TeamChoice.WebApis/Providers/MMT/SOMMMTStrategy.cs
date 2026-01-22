using TeamChoice.WebApis.Models;
using TeamChoice.WebApis.Models.DTOs;

namespace TeamChoice.WebApis.Providers.MMT
{
    public class SOMMMTStrategy : MMTBaseClass, IProviderLookupStrategy
    {
        public SOMMMTStrategy(HttpClient httpClient, ServiceProviderProperties properties)
            : base(httpClient, properties)
        {
        }

        public bool Supports(string serviceCode)
        {
            return "00006" == serviceCode;
        }

        public async Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode)
        {
            var replacedPhone = phoneNumber.Replace("+", "");

            return await base.LookupAsync(
                replacedPhone,
                serviceCode,
                "mmt",
                "SOK",
                "SO"
            );
        }
    }
}
