using Dapper;
using MediatR;
using VaultDomain.Commands.UpdateSecret;
using VaultDomain.ValueObjects;

namespace VaultInfrastructure.Data.Commands.Secret
{
    internal class UpdateSecretCommandHandler : BaseStorageCommandHandler<UpdateSecretCommand, Result>
    {
        public UpdateSecretCommandHandler(DbConnectionString connectionString, IMediator mediator) : base(connectionString, mediator)
        {
        }

        public override async Task<Result> Handle(UpdateSecretCommand request, CancellationToken cancellationToken)
        {
            using (var connection = this.CreateConnection())
            {
                try
                {
                    var secret = request.Materialize();
                    var sql = "UPDATE [Secrets] SET SecretValue = @SecretValue WHERE Id = @Id";
                    await DispatchAllEvents(secret.TakeEvents());
                    var affectedRows = await connection.ExecuteAsync(sql, new
                    {
                        secret.Id,
                        secret.SecretValue
                    });
                    if (affectedRows > 0)
                        return new Result().WithInfo("Secret update successfully");
                    return new Result().WithWarning("Something went wrong while try to save the secret.\nNo data saved");
                }
                catch (Exception ex)
                {
                    return new Result(ex).WithWarning("An error happened during save the secret");
                }
            }
        }
    }
}
