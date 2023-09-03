using Dapper;
using MediatR;
using VaultDomain.Commands.DeleteSecret;
using VaultDomain.ValueObjects;

namespace VaultInfrastructure.Data.Commands.Secret
{
    internal class DeleteSecretCommandHandler : BaseStorageCommandHandler<DeleteSecretCommand, Result>
    {
        public DeleteSecretCommandHandler(DbConnectionString connectionString, IMediator mediator) : base(connectionString, mediator)
        {
        }

        public override async Task<Result> Handle(DeleteSecretCommand request, CancellationToken cancellationToken)
        {
            using (var connection = this.CreateConnection())
            {
                try
                {
                    var secret = request.Materialize();
                    var sql = "DELETE FROM [Secrets] WHERE Id = @Id";
                    await DispatchAllEvents(secret.TakeEvents());
                    var affectedRows = await connection.ExecuteAsync(sql, new
                    {
                        secret.Id,
                        secret.SecretValue
                    });
                    if (affectedRows > 0)
                        return new Result().WithInfo("Secret deleted successfully");
                    return new Result().WithWarning("Something went wrong while try to delete the secret.\nNo data saved");
                }
                catch (Exception ex)
                {
                    return new Result(ex).WithWarning("An error happened during delete the secret");
                }
            }
        }
    }
}
