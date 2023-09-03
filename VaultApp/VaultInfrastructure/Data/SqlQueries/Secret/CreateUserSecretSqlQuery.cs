using UserSecret = VaultDomain.Entities.Secret;
using VaultInfrastructure.Data.Abstractions;

namespace VaultInfrastructure.Data.SqlQueries.Secret
{
    internal class CreateUserSecretSqlQuery : QueryCommand<UserSecret>
    {
        public CreateUserSecretSqlQuery(UserSecret userSecret) : base(userSecret) { }

        public override string ToQuery() =>
            "INSERT INTO [Secrets] VALUES (@Id, @SecretName, @SecretValue, @CreatedAt, @Owner)";
    }
}
