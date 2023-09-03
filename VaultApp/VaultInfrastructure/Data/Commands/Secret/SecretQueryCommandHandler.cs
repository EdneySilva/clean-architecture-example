using MediatR;
using VaultDomain.Queries.Secret;
using VaultDomain.ValueObjects;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Data.Commands.Secret
{
    internal class SecretQueryCommandHandler :
        BaseStorageCommandHandler<UserSecretsQuery, Result>,
        IRequestHandler<UserSecretBySecretNameQuery, VaultDomain.Entities.Secret>
    {
        private readonly IRepository<VaultDomain.Entities.Secret> _secretRepository;

        public SecretQueryCommandHandler(DbConnectionString connectionString, IRepository<VaultDomain.Entities.Secret> secretRepository, IMediator mediator) : base(connectionString, mediator)
        {
            _secretRepository = secretRepository;
        }

        public override async Task<Result> Handle(UserSecretsQuery request, CancellationToken cancellationToken)
        {
            return await _secretRepository.SelectAsync(new UserSecretsSqlQuery(new VaultDomain.Entities.Secret { Owner = request.Owner }));
        }

        public async Task<VaultDomain.Entities.Secret> Handle(UserSecretBySecretNameQuery request, CancellationToken cancellationToken)
        {
            return await _secretRepository.FirstOrDefaultAsync(new UserSecretsBySecretNameSqlQuery(new VaultDomain.Entities.Secret { Owner = request.Owner, SecretName = request.SecretName }));
        }
    }
}
