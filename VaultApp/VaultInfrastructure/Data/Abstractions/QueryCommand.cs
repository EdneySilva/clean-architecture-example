namespace VaultInfrastructure.Data.Abstractions
{
    internal abstract class QueryCommand<TEntity> where TEntity : class
    {
        public QueryCommand(TEntity paramters)
        {
            Parameters = paramters;
        }

        public TEntity Parameters { get; }

        public abstract string ToQuery();
    }
}
