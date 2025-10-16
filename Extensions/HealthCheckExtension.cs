using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TaskManagment.Extensions;

public static class HealthCheckExtension
{
    public static IServiceCollection AddProjectHealthCheck(this IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
        return services;
    }
}
