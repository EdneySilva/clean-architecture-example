using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultDomain.Entities;
using VaultDomain.Queries.User;

namespace VaultInfrastructure.Data.Commands.User
{
    internal class UserQueryCommandHandler : IRequestHandler<UserByUserName, VaultDomain.Entities.User?>
    {
        private readonly DbConnectionString _dbConnectionString;

        public UserQueryCommandHandler(DbConnectionString dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public async Task<VaultDomain.Entities.User?> Handle(UserByUserName request, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_dbConnectionString.Value))
            {
                var sql = "SELECT * FROM [Users] WHERE [UserName] = @UserName";
                var user = await connection.QueryFirstOrDefaultAsync<VaultDomain.Entities.User>(sql, new 
                {
                    request.UserName,
                });
                return user;
            }
        }
    }
}
