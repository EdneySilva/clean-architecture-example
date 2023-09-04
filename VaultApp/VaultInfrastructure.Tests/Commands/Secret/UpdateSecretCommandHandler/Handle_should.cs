using MediatR;
using Moq;
using VaultDomain.Events;
using VaultDomain;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Tests.Commands.Secret.UpdateSecretCommandHandler
{
    public class Handle_should
    {
        [Fact]
        public async Task Execute_UpdateSecretSqlQuery_on_repository()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.Secret>>();
            var mediatorMoq = new Mock<IMediator>();
            var handler = new Data.Commands.Secret.UpdateSecretCommandHandler(
                new Data.DbConnectionString(""), 
                repositoryMoq.Object, 
                mediatorMoq.Object
            );
            var result = await handler.Handle(new VaultDomain.Commands.UpdateSecret.UpdateSecretCommand(
                "user@mail.com", "secret", "value"), 
                CancellationToken.None);
            repositoryMoq.Verify(o => o.UpdateAsync(It.IsAny<UpdateSecretSqlQuery>()), Times.Once);
        }

        [Fact]
        public async Task Dispatche_SecretUpdatedEvent()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.Secret>>();
            var mediatorMoq = new Mock<IMediator>();
            var handler = new Data.Commands.Secret.UpdateSecretCommandHandler(
                 new Data.DbConnectionString(""),
                 repositoryMoq.Object,
                 mediatorMoq.Object
             );
            var result = await handler.Handle(new VaultDomain.Commands.UpdateSecret.UpdateSecretCommand(
                "user@mail.com", "secret", "value"),
                CancellationToken.None);
            mediatorMoq.Verify(o => o.Publish(It.Is<BaseEvent>(f => f.GetType() == typeof(SecretUpdated)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
