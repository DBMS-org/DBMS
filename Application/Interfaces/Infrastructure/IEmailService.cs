namespace Application.Interfaces.Infrastructure
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetCode);
        Task SendEmailAsync(string to, string subject, string body);
    }
} 