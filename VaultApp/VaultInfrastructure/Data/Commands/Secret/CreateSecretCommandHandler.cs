using Dapper;
using MediatR;
using VaultDomain.Commands.CreateSecret;
using VaultDomain.ValueObjects;

namespace VaultInfrastructure.Data.Commands.Secret
{
    internal class CreateSecretCommandHandler : BaseStorageCommandHandler<CreateSecretCommand, Result>
    {
        public CreateSecretCommandHandler(DbConnectionString connectionString, IMediator mediator) : base(connectionString, mediator)
        {
        }

        public override async Task<Result> Handle(CreateSecretCommand request, CancellationToken cancellationToken)
        {
            using (var connection = this.CreateConnection())
            {
                try
                {
                    var secret = request.Materialize();
                    var sql = "INSERT INTO [Secrets] VALUES (@Id, @SecretName, @SecretValue, @CreatedAt, @Owner)";
                    var affectedRows = await connection.ExecuteAsync(sql, new
                    {
                        secret.Id,
                        secret.SecretName,
                        secret.SecretValue,
                        secret.Owner,
                        secret.CreatedAt
                    });
                    await DispatchAllEvents(secret.TakeEvents());
                    if (affectedRows > 0)
                        return new Result().WithInfo("Secret created successfully");
                    return new Result().WithWarning("Something went wrong while try to save the secret.\nNo data saved");
                }
                catch (Exception ex)
                {
                    return new Result(ex).WithWarning("An error happened duing save the secret");
                }
            }
        }
    }
}
