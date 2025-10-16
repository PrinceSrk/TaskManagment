namespace TaskManagment.Extensions;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddProjectLogging(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddDebug();

        return logging;
    }
}
