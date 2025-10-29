using Microsoft.OpenApi.Models;

namespace TaskManagment.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerExtensions(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

        // Add access token header
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Access token. Format: Bearer {token}",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        // Add refresh token header
        c.AddSecurityDefinition("RefreshToken", new OpenApiSecurityScheme
        {
            Description = "Refresh token. Format: raw token string",
            Name = "X-Refresh-Token",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "RefreshToken"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new List<string>()
            },
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "RefreshToken"
                    }
                },
                new List<string>()
            }
        });
    });
        return services;
    }
}
