using UserSecret = VaultDomain.Entities.Secret;
using VaultInfrastructure.Data.Abstractions;

namespace VaultInfrastructure.Data.SqlQueries.Secret
{
    internal class UpdateSecretSqlQuery : QueryCommand<UserSecret>
    {
        public UpdateSecretSqlQuery(UserSecret userSecret) : base(userSecret) { }

        public override string ToQuery() => "UPDATE [Secrets] SET SecretValue = @SecretValue WHERE Id = @Id";
    }
}
