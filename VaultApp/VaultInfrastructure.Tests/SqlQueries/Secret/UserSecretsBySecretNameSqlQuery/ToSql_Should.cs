namespace VaultInfrastructure.Tests.SqlQueries.Secret.UserSecretsBySecretNameSqlQuery
{
    public class ToSql_Should
    {
        [Fact]
        public void Return_query_to_select_a_secret_by_name_and_owner()
        {
            var user = new Data.SqlQueries.Secret.UserSecretsBySecretNameSqlQuery(new VaultDomain.Entities.Secret());
            Assert.Equal(user.ToQuery(), "SELECT * FROM [Secrets] WHERE [Owner] = @Owner and SecretName = @SecretName");
        }
    }
}
