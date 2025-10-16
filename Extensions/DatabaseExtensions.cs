using Microsoft.EntityFrameworkCore;
using TaskManagment.Models;

namespace TaskManagment.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabaseConfig(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<TaskContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        return services;    
    }
}
