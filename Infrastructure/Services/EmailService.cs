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

        public async Task SendNewUserCredentialsEmailAsync(string email, string name, string username, string password)
        {
            try
            {
                // ALWAYS log the credentials for testing/debugging
                _logger.LogWarning($"👤 NEW USER CREDENTIALS for {email}: Username={username}, Password={password}");
                Console.WriteLine($"👤 NEW USER CREDENTIALS for {email}: Username={username}, Password={password}");

                var smtpHost = _configuration["Email:SmtpHost"];
                var smtpPort = _configuration["Email:SmtpPort"];
                var smtpUsername = _configuration["Email:Username"];
                var smtpPassword = _configuration["Email:Password"];
                var fromAddress = _configuration["Email:FromAddress"];
                var fromName = _configuration["Email:FromName"];

                // Try to send email, but don't fail if it doesn't work
                try
                {
                    using var smtpClient = new SmtpClient(smtpHost)
                    {
                        Port = int.Parse(smtpPort ?? "587"),
                        Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromAddress ?? smtpUsername, fromName ?? "DBMS System"),
                        Subject = "Welcome to DBMS System - Your Account Credentials",
                        Body = GetNewUserCredentialsEmailBody(name, username, password),
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation($"✅ New user credentials sent successfully to {email}");
                }
                catch (Exception emailEx)
                {
                    _logger.LogWarning($"⚠️ Email sending failed, but credentials are available in logs: {emailEx.Message}");
                    // Don't throw - the credentials are still valid and logged
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Failed to send new user credentials to {email}");
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

        private string GetNewUserCredentialsEmailBody(string name, string username, string password)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #28a745 0%, #20c997 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .credentials-box {{ background: white; border: 2px solid #28a745; padding: 20px; margin: 20px 0; border-radius: 8px; }}
                        .credential-row {{ display: flex; justify-content: space-between; padding: 10px; margin: 5px 0; background: #f8f9fa; border-radius: 5px; }}
                        .credential-label {{ font-weight: bold; color: #555; }}
                        .credential-value {{ font-family: 'Courier New', monospace; color: #28a745; font-weight: bold; }}
                        .warning {{ background: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .info {{ background: #d1ecf1; border: 1px solid #bee5eb; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>👤 Welcome to DBMS System!</h1>
                        </div>
                        <div class='content'>
                            <p>Hello <strong>{name}</strong>,</p>
                            <p>Your account has been successfully created by the General Manager. Below are your login credentials:</p>

                            <div class='credentials-box'>
                                <div class='credential-row'>
                                    <span class='credential-label'>Username (Email):</span>
                                    <span class='credential-value'>{username}</span>
                                </div>
                                <div class='credential-row'>
                                    <span class='credential-label'>Password:</span>
                                    <span class='credential-value'>{password}</span>
                                </div>
                            </div>

                            <div class='warning'>
                                ⚠️ <strong>Security Notice:</strong> Please change your password after your first login for security purposes.
                            </div>

                            <div class='info'>
                                ℹ️ <strong>Next Steps:</strong>
                                <ol style='margin: 10px 0; padding-left: 20px;'>
                                    <li>Login to the DBMS System using the credentials above</li>
                                    <li>Navigate to your profile settings</li>
                                    <li>Change your password to something secure and memorable</li>
                                </ol>
                            </div>

                            <p>If you did not expect this account creation or have any questions, please contact your administrator immediately.</p>

                            <div class='footer'>
                                <p>Best regards,<br><strong>DBMS System Team</strong></p>
                                <p><small>This is an automated message, please do not reply to this email.</small></p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
        }

        public async Task SendAccountDeactivationEmailAsync(string email, string name)
        {
            try
            {
                // ALWAYS log the deactivation notification for testing/debugging
                _logger.LogWarning($"⏸️ ACCOUNT DEACTIVATION NOTIFICATION for {email}");
                Console.WriteLine($"⏸️ ACCOUNT DEACTIVATION NOTIFICATION for {email}");

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
                        Subject = "Account Deactivated - DBMS System",
                        Body = GetAccountDeactivationEmailBody(name),
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation($"✅ Account deactivation notification sent successfully to {email}");
                }
                catch (Exception emailEx)
                {
                    _logger.LogWarning($"⚠️ Email sending failed, but deactivation notification is logged: {emailEx.Message}");
                    // Don't throw - the notification is still logged
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Failed to send account deactivation notification to {email}");
                throw;
            }
        }

        public async Task SendAccountDeletionEmailAsync(string email, string name)
        {
            try
            {
                // ALWAYS log the deletion notification for testing/debugging
                _logger.LogWarning($"🗑️ ACCOUNT DELETION NOTIFICATION for {email}");
                Console.WriteLine($"🗑️ ACCOUNT DELETION NOTIFICATION for {email}");

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
                        Subject = "Account Deleted - DBMS System",
                        Body = GetAccountDeletionEmailBody(name),
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation($"✅ Account deletion notification sent successfully to {email}");
                }
                catch (Exception emailEx)
                {
                    _logger.LogWarning($"⚠️ Email sending failed, but deletion notification is logged: {emailEx.Message}");
                    // Don't throw - the notification is still logged
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Failed to send account deletion notification to {email}");
                throw;
            }
        }

        private string GetAccountDeactivationEmailBody(string userName)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #ffc107 0%, #ff9800 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .alert-box {{ background: white; border-left: 4px solid #ffc107; padding: 20px; margin: 20px 0; border-radius: 5px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
                        .info {{ background: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .contact-box {{ background: #d1ecf1; border: 1px solid #bee5eb; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Account Deactivated</h1>
                        </div>
                        <div class='content'>
                            <p>Hello <strong>{userName}</strong>,</p>

                            <div class='alert-box'>
                                <p style='margin: 0; font-size: 16px;'>
                                    Your account has been <strong>deactivated</strong> by an admin.
                                </p>
                            </div>

                            <div class='info'>
                                ⚠️ <strong>What this means:</strong>
                                <ul style='margin: 10px 0; padding-left: 20px;'>
                                    <li>You will not be able to log in to the DBMS System</li>
                                    <li>Your account data is still preserved in the system</li>
                                    <li>This action can be reversed by your administrator</li>
                                </ul>
                            </div>

                            <div class='contact-box'>
                                <p style='margin: 0; font-size: 14px;'>
                                    <strong>Need to regain access?</strong><br>
                                    Please contact your system administrator for assistance. Your account can be reactivated if needed.
                                </p>
                            </div>

                            <p>If you believe this action was taken in error or if you have any questions, please contact your system administrator immediately.</p>

                            <div class='footer'>
                                <p>Best regards,<br><strong>DBMS System Team</strong></p>
                                <p><small>This is an automated message, please do not reply to this email.</small></p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
        }

        private string GetAccountDeletionEmailBody(string userName)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #dc3545 0%, #c82333 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .alert-box {{ background: white; border-left: 4px solid #dc3545; padding: 20px; margin: 20px 0; border-radius: 5px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
                        .info {{ background: #d1ecf1; border: 1px solid #bee5eb; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Account Deleted</h1>
                        </div>
                        <div class='content'>
                            <p>Hello <strong>{userName}</strong>,</p>

                            <div class='alert-box'>
                                <p style='margin: 0; font-size: 16px;'>
                                    Your account has been <strong>deleted</strong> by an admin.
                                </p>
                            </div>

                            <div class='info'>
                                ℹ️ <strong>What this means:</strong>
                                <ul style='margin: 10px 0; padding-left: 20px;'>
                                    <li>You no longer have access to the DBMS System</li>
                                    <li>All your account data has been removed from the system</li>
                                    <li>You will not be able to log in with your previous credentials</li>
                                </ul>
                            </div>

                            <p>If you believe this action was taken in error or if you have any questions, please contact your system administrator.</p>

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
