using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CleanArchitecture.Domain.Interfaces.Repositories;

namespace CleanArchitecture.Application.Commands
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            
            if (user == null)
                return false;
                
            // In a real app, validate the token from database/cache
            // For this example we'll just assume it's valid
            
            // Reset the password and unlock the account
            user.IsLocked = false;
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            
            await _userRepository.ResetPasswordAsync(user, request.NewPassword);
            
            return true;
        }
    }
} 