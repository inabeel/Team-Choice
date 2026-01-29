using Microsoft.Extensions.Options;
using TeamChoice.WebApis.Application.Interfaces.Repositories;
using TeamChoice.WebApis.Application.Ports;
using TeamChoice.WebApis.Contracts;
using TeamChoice.WebApis.Domain.Configuration;
using TeamChoice.WebApis.Infrastructure.Clients;
using TeamChoice.WebApis.Infrastructure.Persistence;
using TeamChoice.WebApis.Infrastructure.Providers.Security;
using TeamChoice.WebApis.Infrastructure.Repositories;
using TeamChoice.WebApis.Infrastructure.Services;
using TeamChoice.WebApis.Infrastructure.Transport;

namespace TeamChoice.WebApis.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<TransactionForwardingMapper>();


        // ---- Database ----
        services.AddScoped<IDatabaseService, DatabaseService>();

        services.AddScoped<IAgentTransactionGateway, AgentTransactionGateway>();
        services.AddScoped<IServiceCodeResolver, ServiceCodeResolver>();

        // ---- Repositories ----
        services.AddScoped<ILocServiceRepository, LocServiceRepository>();
        services.AddScoped<IRateRepository, RateRepository>();

        services.AddScoped<ITransactionForwarder, TransactionForwarder>();


        // ---- Security ----
        services.AddSingleton<EncryptDecryptService>();
        // Prefer interface if you introduce one:
        // services.AddSingleton<IApiEncryptor, EncryptDecryptService>();

        // ---- Configuration objects ----
        services.Configure<ClientUrlProperties>(
            configuration.GetSection(ClientUrlProperties.SectionName));

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<ClientUrlProperties>>().Value);

        services.Configure<ServiceProviderProperties>(
            configuration.GetSection("ServiceProviders"));

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<ServiceProviderProperties>>().Value);

        services.Configure<ServiceCatalogOptions>(
            configuration.GetSection("ServiceCatalog"));

        return services;
    }
}
