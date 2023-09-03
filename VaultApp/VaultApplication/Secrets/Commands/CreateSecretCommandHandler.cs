using MediatR;
using VaultDomain.Commands.CreateSecret;
using VaultDomain.Queries.Secret;
using VaultDomain.ValueObjects;

namespace VaultApplication.Secrets.Commands
{
    internal class CreateSecretCommandHandler : IPipelineBehavior<CreateSecretCommand, Result>
    {
        private readonly IMediator _mediator;

        public CreateSecretCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(CreateSecretCommand request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
        {
            try
            {
                var duplicatedSecret = await _mediator.Send(new UserSecretBySecretNameQuery(request.Owner, request.Name));
                if (duplicatedSecret != null)
                    throw new Exception($"A secret with the name {request.Name} already exists for the user {request.Owner}");
                return await next();
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}