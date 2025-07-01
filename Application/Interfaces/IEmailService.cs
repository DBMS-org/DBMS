namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetCode);
        Task SendEmailAsync(string to, string subject, string body);
    }
} 