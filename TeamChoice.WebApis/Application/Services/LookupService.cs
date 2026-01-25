using TeamChoice.WebApis.Contracts;

namespace TeamChoice.WebApis.Application.Services;

public interface ILookupService
{
    Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode);
}

public class LookupService : ILookupService
{
    private readonly IProviderRouter _providerRouter;

    public LookupService(IProviderRouter providerRouter)
    {
        _providerRouter = providerRouter;
    }

    public async Task<ServiceLookupResponse> LookupAsync(string phoneNumber, string serviceCode)
    {
        // Resolve the strategy based on service code
        var strategy = _providerRouter.Resolve(serviceCode);

        // Execute the lookup on the resolved strategy
        return await strategy.LookupAsync(phoneNumber, serviceCode);
    }
}
