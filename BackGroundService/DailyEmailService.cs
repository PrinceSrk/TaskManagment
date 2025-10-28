using TaskManagment.Interfaces;

namespace TaskManagment.BackGroundService;

public class DailyEmailService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DailyEmailService> _logger;

    public DailyEmailService(IServiceProvider serviceProvider, ILogger<DailyEmailService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRunTime = DateTime.Now.AddDays(1).AddHours(8);
            var delay = nextRunTime - now;

            if (delay.TotalMilliseconds > 0)
                await Task.Delay(delay, stoppingToken);

            try 
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    IEmailService emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    IAuthService userService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                    var activeUsers = await userService.GetActiveUsers();

                    foreach(var user in activeUsers)
                    {
                        await emailService.SendEmailAsync(user.Email, "Good Morning", "Here's Daily Updates");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Sending Daily Emails");
            }
        }
    }
}
