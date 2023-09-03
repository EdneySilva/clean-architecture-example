using UserSecret = VaultDomain.Entities.Secret;
using VaultInfrastructure.Data.Abstractions;

namespace VaultInfrastructure.Data.SqlQueries.Secret
{
    internal class DeleteSecretSqlQuery : QueryCommand<UserSecret>
    {
        public DeleteSecretSqlQuery(UserSecret userSecret) : base(userSecret) { }

        public override string ToQuery() => "DELETE FROM [Secrets] WHERE Id = @Id";
    }
}
