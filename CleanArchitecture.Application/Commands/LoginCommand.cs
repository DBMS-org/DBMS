using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Authentication;
using MediatR;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Interfaces.Services;

namespace CleanArchitecture.Application.Commands
{
    public class LoginCommand : IRequest<AuthResultDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            
            if (user == null)
                throw new AuthenticationException("Invalid username or password.");
                
            if (user.IsLocked && user.LockoutEnd > DateTime.UtcNow)
                throw new AuthenticationException("Account is locked. Please try again later or reset your password.");
                
            var isPasswordValid = await _userRepository.CheckPasswordAsync(user, request.Password);
            
            if (!isPasswordValid)
            {
                await _userRepository.UpdateFailedLoginAttemptsAsync(user);
                
                if (user.FailedLoginAttempts >= 5)
                    throw new AuthenticationException("Account has been locked due to multiple failed login attempts.");
                    
                throw new AuthenticationException("Invalid username or password.");
            }
            
            // Reset failed attempts on successful login
            user.FailedLoginAttempts = 0;
            user.LastActivityTime = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            
            var roles = await _userRepository.GetUserRolesAsync(user.Id);
            var token = _tokenService.GenerateJwtToken(user, roles);
            
            return new AuthResultDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Roles = roles.Select(r => r.Name).ToList()
                },
                RefreshToken = Guid.NewGuid().ToString() // In a real app, you'd store this
            };
        }
    }
} 