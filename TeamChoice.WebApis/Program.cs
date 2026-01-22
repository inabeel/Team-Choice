
using Microsoft.Extensions.Options;
using TeamChoice.WebApis.Application;
using TeamChoice.WebApis.Domain.Policies;
using TeamChoice.WebApis.Domain.Processors;
using TeamChoice.WebApis.Domain.Validation;
using TeamChoice.WebApis.Infrastructure.Clients;
using TeamChoice.WebApis.Middlewares;

namespace TeamChoice.WebApis;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddScoped<IServiceLookupPolicy, ServiceLookupPolicy>();
        builder.Services.AddScoped<ITransactionOrchestrator, TransactionOrchestrator>();
        builder.Services.AddScoped<ITransactionValidator, TransactionValidator>();
        builder.Services.AddScoped<ITransactionProcessor, TransactionProcessor>();


        builder.Services.Configure<ServiceCatalogOptions>(
            builder.Configuration.GetSection("ServiceCatalog"));

        builder.Services.AddHttpClient<IServiceCatalogClient, ServiceCatalogClient>(
            (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<ServiceCatalogOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
