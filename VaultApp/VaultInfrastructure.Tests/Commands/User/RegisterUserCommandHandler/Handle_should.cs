using MediatR;
using Moq;
using VaultDomain;
using VaultDomain.Events;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.User;

namespace VaultInfrastructure.Tests.Commands.User.RegisterUserCommandHandler
{
    public class Handle_should
    {
        [Fact]
        public async Task Execute_RegisterUserSqlQuery_on_repository()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.User>>();
            var mediatorMoq = new Mock<IMediator>();
            var handler = new Data.Commands.User.RegisterUserCommandHandler(new Data.DbConnectionString(""), repositoryMoq.Object, mediatorMoq.Object);
            var result = await handler.Handle(new VaultDomain.Commands.RegisterUser.RegisterUserCommand("user@mail.com", "password"), CancellationToken.None);
            repositoryMoq.Verify(o => o.InsertAsync(It.IsAny<RegisterUserSqlQuery>()), Times.Once);
        }

        [Fact]
        public async Task Dispatche_UserCreatedEvent()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.User>>();
            var mediatorMoq = new Mock<IMediator>();
            var handler = new Data.Commands.User.RegisterUserCommandHandler(new Data.DbConnectionString(""), repositoryMoq.Object, mediatorMoq.Object);
            var result = await handler.Handle(new VaultDomain.Commands.RegisterUser.RegisterUserCommand("user@mail.com", "password"), CancellationToken.None);
            mediatorMoq.Verify(o => o.Publish(It.Is<BaseEvent>(f => f.GetType() == typeof(UserCreated)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
