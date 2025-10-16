using AutoMapper;
using TaskManagment.Automapper;

namespace TaskManagment.Extensions;

public static class AutoMapper
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}
