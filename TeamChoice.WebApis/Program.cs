
using Microsoft.Extensions.Options;
using TeamChoice.WebApis.Application;
using TeamChoice.WebApis.Application.Orchestrators;
using TeamChoice.WebApis.Application.Policies;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Application.Validators;
using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Infrastructure.Clients;
using TeamChoice.WebApis.Middlewares;

namespace TeamChoice.WebApis;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.AddJwtAuthConfigurations();

        builder.Services.AddControllers();

        builder.Services.AddScoped<IServiceLookupPolicy, ServiceLookupPolicy>();
        builder.Services.AddScoped<ITransactionOrchestrator, TransactionOrchestrator>();
        builder.Services.AddScoped<ITransactionValidator, TransactionValidator>();
        builder.Services.AddScoped<ITransactionProcessor, TransactionProcessor>();
        builder.Services.AddScoped<IDatabaseService, DatabaseService>();

        builder.Services.Configure<ServiceCatalogOptions>(
            builder.Configuration.GetSection("ServiceCatalog"));

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
