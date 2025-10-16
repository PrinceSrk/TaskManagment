namespace TaskManagment.Extensions;

public static class EnvironmentExtensions
{
    public static void ConfigureEnvironmennt(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowAll");
        app.MapHealthChecks("/health");

    }
}
