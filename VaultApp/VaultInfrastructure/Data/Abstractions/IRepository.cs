using VaultDomain.ValueObjects;

namespace VaultInfrastructure.Data.Abstractions
{
    internal interface IRepository<TEntity> where TEntity : class
    {
        Task<Result> InsertAsync(QueryCommand<TEntity> entity);
        Task<Result> UpdateAsync(QueryCommand<TEntity> entity);
        Task<Result> DeleteAsync(QueryCommand<TEntity> entity);
        Task<TEntity> FirstOrDefaultAsync(QueryCommand<TEntity> entity);
        Task<Result> SelectAsync(QueryCommand<TEntity> entity);
    }
}
