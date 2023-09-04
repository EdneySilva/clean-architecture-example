namespace VaultInfrastructure.Tests.SqlQueries.User.UserByUserNameSqlQuery
{
    public class ToQuery_Should
    {
        [Fact]
        public void Return_query_to_select_an_user_by_UserName()
        {
            var user = new Data.SqlQueries.User.UserByUserNameSqlQuery(new VaultDomain.Entities.User());
            Assert.Equal(user.ToQuery(), "SELECT * FROM [Users] WHERE [UserName] = @UserName");
        }
    }
}
