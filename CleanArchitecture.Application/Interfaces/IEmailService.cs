using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
} 