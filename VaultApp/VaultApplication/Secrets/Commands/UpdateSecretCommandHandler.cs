using MediatR;
using VaultDomain.Commands.UpdateSecret;
using VaultDomain.Queries.Secret;
using VaultDomain.ValueObjects;

namespace VaultApplication.Secrets.Commands
{
    internal class UpdateSecretCommandHandler : IPipelineBehavior<UpdateSecretCommand, Result>
    {
        private readonly IMediator _mediator;

        public UpdateSecretCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(UpdateSecretCommand request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
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