using Dapper;
using MediatR;
using VaultDomain.Queries.Secret;
using VaultDomain.ValueObjects;

namespace VaultInfrastructure.Data.Commands.Secret
{
    internal class SecretQueryCommandHandler : BaseStorageCommandHandler<UserSecretsQuery, Result>
    {
        public SecretQueryCommandHandler(DbConnectionString connectionString, IMediator mediator) : base(connectionString, mediator)
        {
        }

        public override async Task<Result> Handle(UserSecretsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    var query = "SELECT * FROM [Secrets] WHERE [Owner] = @UserName";
                    var queryResult = await connection.QueryAsync<VaultDomain.Entities.Secret>(query, new
                    {
                        UserName = request.Owner
                    });
                    var result = new Result(
                        new 
                        { 
                            TotalRecords = queryResult.Count(),
                            Items = queryResult
                        }
                    );
                    if (!queryResult.Any())
                        result.WithWarning("0 records found.");
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}
