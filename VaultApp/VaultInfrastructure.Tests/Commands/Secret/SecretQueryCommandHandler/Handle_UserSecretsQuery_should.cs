using MediatR;
using Moq;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.Secret;

namespace VaultInfrastructure.Tests.Commands.Secret.SecretQueryCommandHandler
{
    public class Handle_UserSecretsQuery_should
    {
        [Fact]
        public async Task Execute_UserSecretBySecretNameQuery_on_repository_and_return_a_record_when_found()
        {
            var repositoryMoq = new Mock<IRepository<VaultDomain.Entities.Secret>>();
            var mediatorMoq = new Mock<IMediator>();
            var list = new List<VaultDomain.Entities.Secret>
            {
                new VaultDomain.Entities.Secret
                {
                    SecretName = "mysecret",
                    SecretValue = "myvalue",
                    Owner = "user@email.com"
                },
                new VaultDomain.Entities.Secret 
                {
                    SecretName = "mysecret2",
                    SecretValue = "myvalue",
                    Owner = "user@email.com"
                },
                new VaultDomain.Entities.Secret
                {
                    SecretName = "mysecret2",
                    SecretValue = "myvalue",
                    Owner = "user2@email.com"
                }
            };
            repositoryMoq.Setup(p => p.SelectAsync(It.IsAny<UserSecretsSqlQuery>())).Returns<UserSecretsSqlQuery>(async (query) =>
            {
                var queryResult = list.Where(w => w.Owner == query.Parameters.Owner).ToList();
                return new VaultDomain.ValueObjects.Result(new QueryResult
                {
                    TotalRecords = queryResult.Count(),
                    Items = queryResult
                });
            });
            var handler = new Data.Commands.Secret.SecretQueryCommandHandler(
                new Data.DbConnectionString(""), 
                repositoryMoq.Object,
                mediatorMoq.Object
            );
            var result = await handler.Handle(new VaultDomain.Queries.Secret.UserSecretsQuery("user@email.com"), CancellationToken.None);
            repositoryMoq.Verify(o => o.SelectAsync(It.IsAny<UserSecretsSqlQuery>()), Times.Once);
            var payload = (QueryResult)result.Payload;
            Assert.Equal(payload.TotalRecords, 2);
            Assert.Equal(payload.Items.Intersect(list).Count(), 2);
        }

        class QueryResult
        {
            public int TotalRecords { get; set; }
            public IEnumerable<VaultDomain.Entities.Secret> Items { get; set; }
        }
    }
}
