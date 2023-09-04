using MediatR;
using VaultDomain.Commands.UpdateSecret;
using VaultDomain.ValueObjects;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Data.Commands.Secret
{
    internal class UpdateSecretCommandHandler : BaseStorageCommandHandler<UpdateSecretCommand, Result>
    {
        private readonly IRepository<VaultDomain.Entities.Secret> _secretRepository;
        public UpdateSecretCommandHandler(DbConnectionString connectionString, IRepository<VaultDomain.Entities.Secret> secretRepository,  IMediator mediator) : base(connectionString, mediator)
        {
            _secretRepository = secretRepository;
        }

        public override async Task<Result> Handle(UpdateSecretCommand request, CancellationToken cancellationToken)
        {
            var secret = request.Materialize();
            await DispatchAllEvents(secret.TakeEvents());
            return await _secretRepository.UpdateAsync(new UpdateSecretSqlQuery(secret));
        }
    }
}
