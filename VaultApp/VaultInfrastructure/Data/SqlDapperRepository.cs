using Azure.Core;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using VaultDomain.ValueObjects;
using VaultInfrastructure.Data.Abstractions;
using static Dapper.SqlMapper;

namespace VaultInfrastructure.Data
{
    internal class SqlDapperRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbConnectionString _connectionString;

        public SqlDapperRepository(DbConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString.Value);

        public async Task<Result> InsertAsync(QueryCommand<TEntity> command)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var affectedRows = await ExecuteAsync(connection, command);
                    if (affectedRows > 0)
                        return new Result().WithInfo($"{typeof(TEntity).Name} created successfully");
                    return new Result().WithWarning($"Something went wrong while try to save the {typeof(TEntity).Name}.\nNo data saved");
                }
                catch (Exception ex)
                {
                    return new Result(ex)
                        .WithWarning($"An error happened when try to execute the command to insert {typeof(TEntity).Name}");
                }
            }
        }

        public async Task<Result> UpdateAsync(QueryCommand<TEntity> command)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var affectedRows = await ExecuteAsync(connection, command);
                    if (affectedRows > 0)
                        return new Result().WithInfo($"{typeof(TEntity).Name} updated successfully");
                    return new Result().WithWarning($"Something went wrong while try to update the {typeof(TEntity).Name}.\nNo data saved");
                }
                catch (Exception ex)
                {
                    return new Result(ex)
                        .WithWarning($"An error happened when try to execute the command to update {typeof(TEntity).Name}");
                }
            }
        }

        public async Task<Result> DeleteAsync(QueryCommand<TEntity> command)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var affectedRows = await ExecuteAsync(connection, command);
                    if (affectedRows > 0)
                        return new Result().WithInfo($"{typeof(TEntity).Name} deleted successfully");
                    return new Result().WithWarning($"Something went wrong while try to delete the {typeof(TEntity).Name}.\nNo data saved");
                }
                catch (Exception ex)
                {
                    return new Result(ex)
                        .WithWarning($"An error happened when try to execute the command to delete {typeof(TEntity).Name}");
                }
            }
        }

        private static async Task<int> ExecuteAsync(SqlConnection connection, QueryCommand<TEntity> command)
        {
            var sql = command.ToQuery();
            var affectedRows = await connection.ExecuteAsync(sql, command.Parameters);
            return affectedRows;
        }

        public async Task<TEntity> FirstOrDefaultAsync(QueryCommand<TEntity> command)
        {
            using (var connection = CreateConnection())
            {
                var query = command.ToQuery();
                var queryResult = await connection.QueryFirstOrDefaultAsync<TEntity>(query, command.Parameters);
                return queryResult;
            }
        }

        public async Task<Result> SelectAsync(QueryCommand<TEntity> command)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var query = command.ToQuery();
                    var queryResult = await connection.QueryAsync<VaultDomain.Entities.Secret>(query, command.Parameters);
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
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }
    }
}
