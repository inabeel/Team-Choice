using TeamChoice.WebApis.Application;
using TeamChoice.WebApis.Infrastructure.DependencyInjection;
using TeamChoice.WebApis.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddJwtAuthConfigurations();

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddProviderStrategies();
builder.Services.AddExternalClients();
builder.Services.AddConfiguredRateLimiting(builder.Configuration);

builder.Services.AddTawakkalSwaggerSchema();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();