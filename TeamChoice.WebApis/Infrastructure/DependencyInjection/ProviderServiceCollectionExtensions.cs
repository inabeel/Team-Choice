using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Infrastructure.Providers.MMT;
using TeamChoice.WebApis.Infrastructure.Providers.SomBank;
using TeamChoice.WebApis.Infrastructure.Providers.TPlus;

namespace TeamChoice.WebApis.Infrastructure.DependencyInjection;

public static class ProviderServiceCollectionExtensions
{
    public static IServiceCollection AddProviderStrategies(this IServiceCollection services)
    {
        // Lookup strategies (IEnumerable<IProviderLookupStrategy>)
        services.AddHttpClient<IProviderLookupStrategy, MPesaMMTStrategy>();
        services.AddHttpClient<IProviderLookupStrategy, SOMMMTStrategy>();
        services.AddHttpClient<IProviderLookupStrategy, SomBankLookupStrategy>();
        services.AddHttpClient<IProviderLookupStrategy, TplusLookupStrategy>();

        return services;
    }

    public static IServiceCollection AddExternalClients(this IServiceCollection services)
    {
        services.AddHttpClient<IAccountLookupService, AccountLookupService>();

        // Enable when needed
        // services.AddHttpClient<ITransactionForwarder, TransactionForwarder>();
        // services.AddHttpClient<ITransactionCallbackClient, TransactionCallbackClient>();

        return services;
    }
}
