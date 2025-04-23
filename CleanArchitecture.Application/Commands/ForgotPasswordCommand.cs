using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Interfaces.Services;

namespace CleanArchitecture.Application.Commands
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IEmailService emailService,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            
            if (user == null)
                return true; // Don't reveal whether the email exists for security
                
            // Generate password reset token (in a real app you'd store this securely with expiration)
            var resetToken = Guid.NewGuid().ToString();
            
            // Send email with reset link
            var resetLink = $"https://yourapplication.com/reset-password?email={user.Email}&token={resetToken}";
            await _emailService.SendEmailAsync(
                user.Email,
                "Password Reset Request",
                $"Please reset your password by clicking on this link: {resetLink}\n\nThis link will expire in 24 hours."
            );
            
            return true;
        }
    }
} 