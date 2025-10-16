using TaskManagment.Repository;

namespace TaskManagment.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddScopedRepository(this IServiceCollection services)
    {
        services.AddScoped<IAuthRepo, AuthRepo>();
        services.AddScoped<ITaskRepo, TaskRepo>();
        return services;
    }   
}
