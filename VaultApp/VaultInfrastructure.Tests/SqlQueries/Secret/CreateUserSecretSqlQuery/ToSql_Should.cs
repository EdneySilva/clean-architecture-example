namespace VaultInfrastructure.Tests.SqlQueries.Secret.CreateUserSecretSqlQuery
{
    public class ToSql_Should
    {
        [Fact]
        public void Return_query_to_insert_a_secret()
        {
            var user = new Data.SqlQueries.Secret.CreateUserSecretSqlQuery(new VaultDomain.Entities.Secret());
            Assert.Equal(user.ToQuery(), "INSERT INTO [Secrets] VALUES (@Id, @SecretName, @SecretValue, @CreatedAt, @Owner)");
        }
    }
}
