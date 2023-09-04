namespace VaultInfrastructure.Tests.SqlQueries.Secret.UpdateSecretSqlQuery
{
    public class ToSql_Should
    {
        [Fact]
        public void Return_query_to_update_a_secret()
        {
            var user = new Data.SqlQueries.Secret.UpdateSecretSqlQuery(new VaultDomain.Entities.Secret());
            Assert.Equal(user.ToQuery(), "UPDATE [Secrets] SET SecretValue = @SecretValue WHERE Id = @Id");
        }
    }
}
