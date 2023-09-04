using Moq;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.User;

namespace VaultInfrastructure.Tests.Commands.User.UserQueryCommandHandler
{
    public class Handle_should
    {
        [Fact]
        public async Task Execute_UserQueryCommandHandler_on_repository_and_return_a_record_when_found()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.User>>();
            var list = new List<VaultDomain.Entities.User>();
            list.Add(new VaultDomain.Entities.User { UserName = "user@email.com" });
            repositoryMoq.Setup(p => p.FirstOrDefaultAsync(It.IsAny<UserByUserNameSqlQuery>())).Returns<UserByUserNameSqlQuery>(async (query) =>
            {
                return list.FirstOrDefault(w => w.UserName == query.Parameters.UserName);
            });
            var handler = new Data.Commands.User.UserQueryCommandHandler(repositoryMoq.Object);
            var result = await handler.Handle(new VaultDomain.Queries.User.UserByUserName("user@email.com"), CancellationToken.None);
            repositoryMoq.Verify(o => o.FirstOrDefaultAsync(It.IsAny<UserByUserNameSqlQuery>()), Times.Once);
            Assert.Equal(result, list.First());
        }

        [Fact]
        public async Task Execute_UserQueryCommandHandler_on_repository_and_return_null_if_not_found()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.User>>();
            var list = new List<VaultDomain.Entities.User>();
            list.Add(new VaultDomain.Entities.User { UserName = "notfouonduser@email.com" });
            repositoryMoq.Setup(p => p.FirstOrDefaultAsync(It.IsAny<UserByUserNameSqlQuery>())).Returns<UserByUserNameSqlQuery>(async (query) =>
            {
                return list.FirstOrDefault(w => w.UserName == query.Parameters.UserName);
            });
            var handler = new Data.Commands.User.UserQueryCommandHandler(repositoryMoq.Object);
            var result = await handler.Handle(new VaultDomain.Queries.User.UserByUserName("user@email.com"), CancellationToken.None);
            repositoryMoq.Verify(o => o.FirstOrDefaultAsync(It.IsAny<UserByUserNameSqlQuery>()), Times.Once);
            Assert.Null(result);
        }
    }
}
