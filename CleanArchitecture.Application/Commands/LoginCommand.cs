using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Authentication;
using MediatR;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Commands
{
    public class LoginCommand : IRequest<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, User>
    {
        public async Task<User> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implement direct user authentication logic here
            return new User
            {
                Username = request.Username,
                Email = $"{request.Username}@example.com" // Placeholder
            };
        }
    }
} 