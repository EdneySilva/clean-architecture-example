using UserSecret = VaultDomain.Entities.Secret;
using VaultInfrastructure.Data.Abstractions;

namespace VaultInfrastructure.Data.SqlQueries.Secret
{
    internal class UserSecretsSqlQuery : QueryCommand<UserSecret>
    {
        public UserSecretsSqlQuery(UserSecret userSecret) : base(userSecret) { }

        public override string ToQuery() => "SELECT * FROM [Secrets] WHERE [Owner] = @Owner";
    }
}
