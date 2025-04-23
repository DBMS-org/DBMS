// Infrastructure/Services/EmailService.cs
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using CleanArchitecture.Application.Interfaces;

namespace CleanArchitecture.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpServer = _configuration["Email:SmtpServer"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
            var smtpUsername = _configuration["Email:Username"];
            var smtpPassword = _configuration["Email:Password"];
            var senderEmail = _configuration["Email:SenderEmail"];
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("System", senderEmail));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
