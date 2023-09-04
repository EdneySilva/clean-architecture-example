namespace VaultInfrastructure.Tests.SqlQueries.Secret.DeleteSecretSqlQuery
{
    public class ToSql_Should
    {
        [Fact]
        public void Return_query_to_delete_secret()
        {
            var user = new Data.SqlQueries.Secret.DeleteSecretSqlQuery(new VaultDomain.Entities.Secret());
            Assert.Equal(user.ToQuery(), "DELETE FROM [Secrets] WHERE Id = @Id");
        }
    }
}
