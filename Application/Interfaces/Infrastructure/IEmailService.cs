namespace Application.Interfaces.Infrastructure
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetCode);
        Task SendEmailAsync(string to, string subject, string body);
        Task SendNewUserCredentialsEmailAsync(string email, string name, string username, string password);
        Task SendAccountDeactivationEmailAsync(string email, string name);
        Task SendAccountDeletionEmailAsync(string email, string name);
    }
} 