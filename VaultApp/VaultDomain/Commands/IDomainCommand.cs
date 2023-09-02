using MediatR;

namespace VaultDomain.Commands
{
    public interface IDomainCommand<TResult> : IRequest<TResult>
    {

    }
}
