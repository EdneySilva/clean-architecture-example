namespace VaultInfrastructure.Tests.SqlQueries.User.RegisterUserSqlQuery
{
    public class ToQuery_Should
    {
        [Fact]
        public void Return_query_to_insert_an_user()
        {
            var user = new Data.SqlQueries.User.RegisterUserSqlQuery(new VaultDomain.Entities.User());
            Assert.Equal(user.ToQuery(), "INSERT INTO [Users] VALUES (@UserName, @Password, @CreatedAt, @Status)");
        }
    }
}
