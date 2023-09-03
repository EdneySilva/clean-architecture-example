using VaultUser = VaultDomain.Entities.User;
using VaultInfrastructure.Data.Abstractions;

namespace VaultInfrastructure.Data.SqlQueries.User
{
    internal class RegisterUserSqlQuery : QueryCommand<VaultUser>
    {
        public RegisterUserSqlQuery(VaultUser paramters) : base(paramters)
        {
        }

        public override string ToQuery() => "INSERT INTO [Users] VALUES (@UserName, @Password, @CreatedAt, @Status)";
    }
}
