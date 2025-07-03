using Application.Interfaces.Infrastructure;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetCode)
        {
            try
            {
                // ALWAYS log the code for testing/debugging
                _logger.LogWarning($"🔐 PASSWORD RESET CODE for {email}: {resetCode}");
                Console.WriteLine($"🔐 PASSWORD RESET CODE for {email}: {resetCode}");
                
                var smtpHost = _configuration["Email:SmtpHost"];
                var smtpPort = _configuration["Email:SmtpPort"];
                var username = _configuration["Email:Username"];
                var password = _configuration["Email:Password"];
                var fromAddress = _configuration["Email:FromAddress"];
                var fromName = _configuration["Email:FromName"];

                // Try to send email, but don't fail if it doesn't work
                try
                {
                    using var smtpClient = new SmtpClient(smtpHost)
                    {
                        Port = int.Parse(smtpPort ?? "587"),
                        Credentials = new NetworkCredential(username, password),
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromAddress ?? username, fromName ?? "DBMS System"),
                        Subject = "Password Reset Code - DBMS System",
                        Body = GetPasswordResetEmailBody("User", resetCode),
                        IsBodyHtml = true,
                    };
                    
                    mailMessage.To.Add(email);
                    
                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation($"✅ Password reset code sent successfully to {email}");
                }
                catch (Exception emailEx)
                {
                    _logger.LogWarning($"⚠️ Email sending failed, but code is available in logs: {emailEx.Message}");
                    // Don't return false - the code is still valid and logged
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Failed to generate password reset code for {email}");
                throw;
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpHost = _configuration["Email:SmtpHost"];
                var smtpPort = _configuration["Email:SmtpPort"];
                var username = _configuration["Email:Username"];
                var password = _configuration["Email:Password"];
                var fromAddress = _configuration["Email:FromAddress"];
                var fromName = _configuration["Email:FromName"];

                using var smtpClient = new SmtpClient(smtpHost)
                {
                    Port = int.Parse(smtpPort ?? "587"),
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress ?? username, fromName ?? "DBMS System"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                
                mailMessage.To.Add(to);
                
                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"✅ Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Failed to send email to {to}");
                throw;
            }
        }

        private string GetPasswordResetEmailBody(string userName, string code)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .code-box {{ background: white; border: 2px dashed #4a90e2; padding: 20px; text-align: center; margin: 20px 0; border-radius: 8px; }}
                        .code {{ font-size: 32px; font-weight: bold; color: #4a90e2; letter-spacing: 8px; font-family: 'Courier New', monospace; }}
                        .warning {{ background: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🔐 Password Reset Request</h1>
                        </div>
                        <div class='content'>
                            <p>Hello <strong>{userName}</strong>,</p>
                            <p>We received a request to reset your password for your DBMS System account.</p>
                            
                            <div class='code-box'>
                                <p>Your verification code is:</p>
                                <div class='code'>{code}</div>
                            </div>
                            
                            <div class='warning'>
                                ⚠️ <strong>Important:</strong> This code will expire in <strong>10 minutes</strong> for security reasons.
                            </div>
                            
                            <p>Enter this code on the verification page to proceed with resetting your password.</p>
                            
                            <p>If you did not request this password reset, please ignore this email. Your account remains secure.</p>
                            
                            <div class='footer'>
                                <p>Best regards,<br><strong>DBMS System Team</strong></p>
                                <p><small>This is an automated message, please do not reply to this email.</small></p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
} 
