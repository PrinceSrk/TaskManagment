using System.Net;
using System.Net.Mail;
using TaskManagment.Interfaces;

namespace TaskManagment.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration,ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public async Task SendEmailAsync(string emailTo, string subject, string body)
    {
        MailAddress fromEmailAddress = new MailAddress(_configuration["Smtp:From"] ?? string.Empty);
        MailAddress toMailAddress = new MailAddress(emailTo);
        string password = _configuration["Smtp:Password"] ?? string.Empty;

        SmtpClient smtpClient = new SmtpClient
        {
            Host = _configuration["Smtp:Host"] ?? string.Empty,
            Port = int.Parse(_configuration["Smtp:Port"] ?? string.Empty),
            EnableSsl = true,
            Credentials = new NetworkCredential(fromEmailAddress.Address, password)
        };

        using var message = new MailMessage(fromEmailAddress, toMailAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(emailTo);

        try
        {
            await smtpClient.SendMailAsync(message);
            _logger.LogInformation($"Email sent to {emailTo}");
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"Failed to Sent email {emailTo}");
        }
    }
}
