using TaskManagment.Interfaces;
using TaskManagment.Services;

namespace TaskManagment.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddScopedBusinnessService(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITaskService, TaskService>();
        return services;
    }
}
