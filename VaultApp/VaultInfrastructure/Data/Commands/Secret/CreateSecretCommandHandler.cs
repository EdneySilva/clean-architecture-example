using MediatR;
using VaultDomain.Commands.CreateSecret;
using VaultDomain.ValueObjects;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Data.Commands.Secret
{
    internal class CreateSecretCommandHandler : BaseStorageCommandHandler<CreateSecretCommand, Result>
    {
        private IRepository<VaultDomain.Entities.Secret> _secretRepository;
        public CreateSecretCommandHandler(DbConnectionString connectionString, IRepository<VaultDomain.Entities.Secret> secretRepository, IMediator mediator) : base(connectionString, mediator)
        {
            _secretRepository = secretRepository;
        }

        public override async Task<Result> Handle(CreateSecretCommand request, CancellationToken cancellationToken)
        {
            var secret = request.Materialize();
            await DispatchAllEvents(secret.TakeEvents());
            return await _secretRepository.InsertAsync(new CreateUserSecretSqlQuery(secret));
        }
    }
}
