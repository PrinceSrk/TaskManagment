using Microsoft.AspNetCore.Authentication;
using TaskManagment.Middleware;

namespace TaskManagment.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationExtension(this IServiceCollection services)
    {
        services.AddAuthentication("CustomToken")
             .AddScheme<AuthenticationSchemeOptions, DummyAuthenticationHandler>("CustomToken", options =>
                {
                    options.TimeProvider = TimeProvider.System;
                });

        return services;
    }
}
