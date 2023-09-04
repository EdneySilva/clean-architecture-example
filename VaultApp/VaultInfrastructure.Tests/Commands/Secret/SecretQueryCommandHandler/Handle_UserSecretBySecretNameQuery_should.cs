using MediatR;
using Moq;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Tests.Commands.Secret.SecretQueryCommandHandler
{
    public class Handle_UserSecretBySecretNameQuery_should
    {
        [Fact]
        public async Task Execute_UserSecretBySecretNameQuery_on_repository_and_return_a_record_when_found()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.Secret>>();
            var mediatorMoq = new Mock<IMediator>();
            var list = new List<VaultDomain.Entities.Secret>();
            list.Add(new VaultDomain.Entities.Secret 
            { 
                SecretName = "mysecret",
                SecretValue = "myvalue",
                Owner = "user@email.com" 
            });
            repositoryMoq.Setup(p => p.FirstOrDefaultAsync(It.IsAny<UserSecretsBySecretNameSqlQuery>())).Returns<UserSecretsBySecretNameSqlQuery>(async (query) =>
            {
                return list.FirstOrDefault(w => w.Owner == query.Parameters.Owner && w.SecretName == query.Parameters.SecretName);
            });
            var handler = new Data.Commands.Secret.SecretQueryCommandHandler(
                new Data.DbConnectionString(""), 
                repositoryMoq.Object,
                mediatorMoq.Object
            );
            var result = await handler.Handle(new VaultDomain.Queries.Secret.UserSecretBySecretNameQuery("user@email.com", "mysecret"), CancellationToken.None);
            repositoryMoq.Verify(o => o.FirstOrDefaultAsync(It.IsAny<UserSecretsBySecretNameSqlQuery>()), Times.Once);
            Assert.Equal(result, list.First());
        }
    }
}