using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Commands
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implement password reset logic here
            return true;
        }
    }
} 