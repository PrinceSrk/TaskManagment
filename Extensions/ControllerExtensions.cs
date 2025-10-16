namespace TaskManagment.Extensions;

public static class ControllerExtensions
{
    public static IServiceCollection AddControllerConfig(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });
        return services;
    }
}
