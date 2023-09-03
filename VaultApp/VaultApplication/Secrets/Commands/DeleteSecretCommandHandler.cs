using MediatR;
using VaultDomain.Commands.DeleteSecret;
using VaultDomain.Queries.Secret;
using VaultDomain.ValueObjects;

namespace VaultApplication.Secrets.Commands
{
    internal class DeleteSecretCommandHandler : IPipelineBehavior<DeleteSecretCommand, Result>
    {
        private readonly IMediator _mediator;

        public DeleteSecretCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(DeleteSecretCommand request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
        {
            try
            {
                var secretToUpdate = await _mediator.Send(new UserSecretBySecretNameQuery(request.Owner, request.Name));
                if (secretToUpdate == null)
                    throw new Exception($"Secret not found with the name {request.Name} for the user {request.Owner}");
                request.Id = secretToUpdate.Id;
                return await next();
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}