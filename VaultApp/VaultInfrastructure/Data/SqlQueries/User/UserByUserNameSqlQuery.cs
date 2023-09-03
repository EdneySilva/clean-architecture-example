using VaultUser = VaultDomain.Entities.User;
using VaultInfrastructure.Data.Abstractions;

namespace VaultInfrastructure.Data.SqlQueries.User
{
    internal class UserByUserNameSqlQuery : QueryCommand<VaultUser>
    {
        public UserByUserNameSqlQuery(VaultUser paramters) : base(paramters)
        {
        }

        public override string ToQuery() => "SELECT * FROM [Users] WHERE [UserName] = @UserName";
    }
}
