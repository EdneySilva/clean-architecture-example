using MediatR;
using Moq;
using VaultDomain;
using VaultDomain.Events;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Tests.Commands.Secret.DeleteSecretCommandHandler
{
    public class Handle_should
    {
        [Fact]
        public async Task Execute_DeleteSecretSqlQuery_on_repository()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.Secret>>();
            var mediatorMoq = new Mock<IMediator>();
            var handler = new Data.Commands.Secret.DeleteSecretCommandHandler(
                new Data.DbConnectionString(""),
                repositoryMoq.Object,
                mediatorMoq.Object
            );
            var result = await handler.Handle(new VaultDomain.Commands.DeleteSecret.DeleteSecretCommand(
                "user@mail.com", "secret"),
                CancellationToken.None);
            repositoryMoq.Verify(o => o.DeleteAsync(It.IsAny<DeleteSecretSqlQuery>()), Times.Once);
        }

        [Fact]
        public async Task Dispatche_SecretDeletedEvent()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.Secret>>();
            var mediatorMoq = new Mock<IMediator>();
            var handler = new Data.Commands.Secret.DeleteSecretCommandHandler(
                 new Data.DbConnectionString(""),
                 repositoryMoq.Object,
                 mediatorMoq.Object
             );
            var result = await handler.Handle(new VaultDomain.Commands.DeleteSecret.DeleteSecretCommand(
                "user@mail.com", "secret"),
                CancellationToken.None);
            mediatorMoq.Verify(o => o.Publish(It.Is<BaseEvent>(f => f.GetType() == typeof(SecretDeleted)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
