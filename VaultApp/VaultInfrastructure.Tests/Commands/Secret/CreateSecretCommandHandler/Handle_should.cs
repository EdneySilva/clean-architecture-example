using MediatR;
using Moq;
using VaultDomain.Events;
using VaultDomain;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Tests.Commands.Secret.CreateSecretCommandHandler
{
    public class Handle_should
    {
        [Fact]
        public async Task Execute_CreateUserSecretSqlQuery_on_repository()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.Secret>>();
            var mediatorMoq = new Mock<IMediator>();
            var handler = new Data.Commands.Secret.CreateSecretCommandHandler(
                new Data.DbConnectionString(""), 
                repositoryMoq.Object, 
                mediatorMoq.Object
            );
            var result = await handler.Handle(new VaultDomain.Commands.CreateSecret.CreateSecretCommand(
                "user@mail.com", "secret", "value"), 
                CancellationToken.None);
            repositoryMoq.Verify(o => o.InsertAsync(It.IsAny<CreateUserSecretSqlQuery>()), Times.Once);
        }

        [Fact]
        public async Task Dispatche_SecretCreatedEvent()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.Secret>>();
            var mediatorMoq = new Mock<IMediator>();
            var handler = new Data.Commands.Secret.CreateSecretCommandHandler(
                new Data.DbConnectionString(""),
                repositoryMoq.Object,
                mediatorMoq.Object
            );
            var result = await handler.Handle(new VaultDomain.Commands.CreateSecret.CreateSecretCommand(
                "user@mail.com", "secret", "value"),
                CancellationToken.None);
            mediatorMoq.Verify(o => o.Publish(It.Is<BaseEvent>(f => f.GetType() == typeof(SecretCreated)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
