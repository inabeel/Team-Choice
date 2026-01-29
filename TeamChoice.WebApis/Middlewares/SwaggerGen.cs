using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Runtime.CompilerServices;

namespace TeamChoice.WebApis.Middlewares;

public static class SwaggerGen
{
    public static IServiceCollection AddTawakkalSwaggerSchema(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName);

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Tawakal API",
                Version = "v1",
                Description = "API documentation for service endpoints."
            });

            c.AddSecurityDefinition("bearer-jwt", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearer-jwt"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
