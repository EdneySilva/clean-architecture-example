namespace VaultInfrastructure.Tests.SqlQueries.Secret.UserSecretsSqlQuery
{
    public class ToSql_Should
    {
        [Fact]
        public void Return_query_to_select_all_user_secrets()
        {
            var user = new Data.SqlQueries.Secret.UserSecretsSqlQuery(new VaultDomain.Entities.Secret());
            Assert.Equal(user.ToQuery(), "SELECT * FROM [Secrets] WHERE [Owner] = @Owner");
        }
    }
}
