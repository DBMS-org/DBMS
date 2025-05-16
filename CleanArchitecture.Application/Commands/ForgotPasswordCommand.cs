using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Commands
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implement password reset logic here
            return true;
        }
    }
} 