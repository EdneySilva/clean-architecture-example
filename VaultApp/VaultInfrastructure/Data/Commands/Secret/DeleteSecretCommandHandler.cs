using Dapper;
using MediatR;
using VaultDomain.Commands.DeleteSecret;
using VaultDomain.ValueObjects;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Data.Commands.Secret
{
    internal class DeleteSecretCommandHandler : BaseStorageCommandHandler<DeleteSecretCommand, Result>
    {
        private readonly IRepository<VaultDomain.Entities.Secret> _secretRepository;

        public DeleteSecretCommandHandler(DbConnectionString connectionString, IRepository<VaultDomain.Entities.Secret> secretRepository, IMediator mediator) : base(connectionString, mediator)
        {
            _secretRepository = secretRepository;
        }

        public override async Task<Result> Handle(DeleteSecretCommand request, CancellationToken cancellationToken)
        {
            var secret = request.Materialize();
            return await _secretRepository.DeleteAsync(new DeleteSecretSqlQuery(secret));
        }
    }
}
