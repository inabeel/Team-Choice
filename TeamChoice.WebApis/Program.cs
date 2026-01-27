using Microsoft.Extensions.Options;
using TeamChoice.WebApis.Application;
using TeamChoice.WebApis.Application.Facades;
using TeamChoice.WebApis.Application.Interfaces.Repositories;
using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Application.Orchestrators;
using TeamChoice.WebApis.Application.Policies;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Application.Validators;
using TeamChoice.WebApis.Contracts;
using TeamChoice.WebApis.Domain.Configuration;
using TeamChoice.WebApis.Infrastructure;
using TeamChoice.WebApis.Infrastructure.Clients;
using TeamChoice.WebApis.Infrastructure.Persistence;
using TeamChoice.WebApis.Infrastructure.Providers.MMT;
using TeamChoice.WebApis.Infrastructure.Providers.Security;
using TeamChoice.WebApis.Infrastructure.Providers.SomBank;
using TeamChoice.WebApis.Infrastructure.Providers.TPlus;
using TeamChoice.WebApis.Infrastructure.Repositories;
using TeamChoice.WebApis.Middlewares;

using TeamChoice.WebApis.Utils;


namespace TeamChoice.WebApis;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // builder.AddJwtAuthConfigurations();

        builder.Services.AddControllers();

        // --- Configuration ---
        builder.Services.Configure<ClientUrlProperties>(
            builder.Configuration.GetSection(ClientUrlProperties.SectionName));

        // Register ServiceProviderProperties
        // Assumes "service:providers" section in appsettings.json matches the structure
        builder.Services.Configure<ServiceProviderProperties>(
             builder.Configuration.GetSection("ServiceProviders"));

        // Explicitly register the object so it can be injected directly (IOptions pattern alternative)
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<ServiceProviderProperties>>().Value);

        builder.Services.Configure<ServiceCatalogOptions>(
            builder.Configuration.GetSection("ServiceCatalog"));

        // --- Infrastructure & Database ---
        builder.Services.AddScoped<IDatabaseService, DatabaseService>();

        // --- Security ---
        // Register EncryptDecryptService as a Singleton (since it holds a key and is stateless/thread-safe)
        // or Scoped/Transient depending on your preference. Singleton is usually fine for crypto services.
        builder.Services.AddSingleton<EncryptDecryptService>();
        // If you use interface injection:
        // builder.Services.AddSingleton<IApiEncryptor, EncryptDecryptService>();

        // --- Repositories ---
        builder.Services.AddScoped<IAgentRepository, AgentRepository>();
        builder.Services.AddScoped<ILocServiceRepository, LocServiceRepository>();
        builder.Services.AddScoped<IRateRepository, RateRepository>();

        // --- Core Business Services ---
        builder.Services.AddScoped<IAgentTransactionFacade, AgentTransactionFacade>();
        //builder.Services.AddScoped<ITransactionService, TransactionService>();
        //builder.Services.AddScoped<IRemittanceService, RemittanceService>();
        builder.Services.AddScoped<ICancellationService, CancellationService>();
        builder.Services.AddScoped<ILookupService, LookupService>();
        builder.Services.AddScoped<ILocServicesService, LocServicesService>();
        builder.Services.AddScoped<IProviderRouter, ProviderRouter>();

        // --- Orchestrators & Validators (Existing) ---
        builder.Services.AddScoped<IServiceLookupPolicy, ServiceLookupPolicy>();
        builder.Services.AddScoped<ITransactionOrchestrator, TransactionOrchestrator>();
        builder.Services.AddScoped<ITransactionValidator, TransactionValidator>();
        builder.Services.AddScoped<ITransactionProcessor, TransactionProcessor>();

        // --- HTTP Client Services ---
        // Registers the service with an injected HttpClient configured from IHttpClientFactory

        builder.Services.AddHttpClient<IAccountLookupService, AccountLookupService>();
        //builder.Services.AddHttpClient<ITransactionForwarder, TransactionForwarder>();
        //builder.Services.AddHttpClient<ITransactionCallBack, TransactionCallBack>();

        //// Notification Services
        //builder.Services.AddHttpClient<IOneSignalService, OneSignalService>();
        //builder.Services.AddHttpClient<ISendTeamsMessage, SendTeamsMessage>();

        // --- Provider Strategies ---
        // Registering multiple implementations of the same interface allows injection of IEnumerable<IProviderLookupStrategy>
        builder.Services.AddHttpClient<IProviderLookupStrategy, MPesaMMTStrategy>();
        builder.Services.AddHttpClient<IProviderLookupStrategy, SOMMMTStrategy>();
        builder.Services.AddHttpClient<IProviderLookupStrategy, SomBankLookupStrategy>();
        builder.Services.AddHttpClient<IProviderLookupStrategy, TplusLookupStrategy>();

        // --- Existing Service Catalog Client ---
        builder.Services.AddHttpClient<IServiceCatalogClient, ServiceCatalogClient>(
            (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<ServiceCatalogOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

        builder.Services.AddTawakkalSwaggerSchema();

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //app.UseAuthentication();
        //app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}