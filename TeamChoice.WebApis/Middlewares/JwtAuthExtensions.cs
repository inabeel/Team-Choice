using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TeamChoice.WebApis.Middlewares;

public static class JwtAuthExtensions
{
    public static WebApplicationBuilder AddJwtAuthConfigurations(this WebApplicationBuilder builder)
    {
        // 🔎 TEMP: enable detailed JWT validation errors
        // IdentityModelEventSource.ShowPII = true;

        builder.Services.Configure<AzureAdOptions>(builder.Configuration.GetSection("AzureAd"));

        var azureAd = builder.Configuration.GetSection("AzureAd").Get<AzureAdOptions>()!;

        var authority = $"https://login.microsoftonline.com/{azureAd.TenantId}";
        var issuer = azureAd.Issuer; // sts.windows.net/{tenantId}

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = azureAd.Audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = false,
                    ValidAudience = azureAd.Audience,

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuthorization();

        return builder;
    }
}

public class AzureAdOptions
{
    public string TenantId { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
}