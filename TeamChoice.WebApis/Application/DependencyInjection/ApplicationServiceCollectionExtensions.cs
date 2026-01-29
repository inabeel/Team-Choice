using TeamChoice.WebApis.Application.Facades;
using TeamChoice.WebApis.Application.Interfaces.Repositories;
using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Application.Mappers;
using TeamChoice.WebApis.Application.Orchestrators;
using TeamChoice.WebApis.Application.Policies;
using TeamChoice.WebApis.Application.Ports;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Domain.Services.Transactions;
using TeamChoice.WebApis.Infrastructure.Persistence;
using TeamChoice.WebApis.Infrastructure.Repositories;
using TeamChoice.WebApis.Infrastructure.Services;
using TeamChoice.WebApis.Infrastructure.Transport;

namespace TeamChoice.WebApis.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<TransactionValidator>();
        services.AddScoped<TransactionFactory>();
        services.AddScoped<TransactionWorkflowService>();
        services.AddScoped<TransactionCallbackMapper>();

        services.AddScoped<TransactionRequestMapper>();
        services.AddScoped<TransactionResultMapper>();

        services.AddScoped<ISendTeamsMessage, SendTeamsMessage>();

        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<IRemittanceService, RemittanceService>();

        services.AddScoped<ITransactionCallbackClient, TransactionCallbackClient>();

        services.AddScoped<IAgentRepository, AgentRepository>();

        // Facades
        services.AddScoped<ITransactionFacade, TransactionFacade>();
        services.AddScoped<IAgentTransactionFacade, AgentTransactionFacade>();


        // Core business services
        services.AddScoped<ILookupService, LookupService>();
        services.AddScoped<ILocServicesService, LocServicesService>();
        services.AddScoped<IProviderRouter, ProviderRouter>();

        // Policies / Validators
        services.AddScoped<IServiceLookupPolicy, ServiceLookupPolicy>();

        services.AddScoped<IRateOrchestrator, RateOrchestrator>();

        return services;
    }
}
