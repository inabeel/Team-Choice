using TeamChoice.WebApis.Application.Interfaces.Services;

namespace TeamChoice.WebApis.Application.Services
{
    public interface IProviderRouter
    {
        IProviderLookupStrategy Resolve(string serviceCode);
    }
    public class ProviderRouter : IProviderRouter
    {
        private readonly IEnumerable<IProviderLookupStrategy> _strategies;

        public ProviderRouter(IEnumerable<IProviderLookupStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IProviderLookupStrategy Resolve(string serviceCode)
        {
            return _strategies.FirstOrDefault(s => s.Supports(serviceCode))
                   ?? throw new ArgumentException($"Invalid service code: {serviceCode}");
        }
    }
}
