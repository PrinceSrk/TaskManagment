namespace TaskManagment.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string emaiTo, string subject, string body);
}
