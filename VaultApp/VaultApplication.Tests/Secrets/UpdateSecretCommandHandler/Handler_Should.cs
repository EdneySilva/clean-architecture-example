﻿using MediatR;
using Moq;
using VaultDomain.Entities;
using VaultDomain.Queries.Secret;

namespace VaultApplication.Tests.Secrets.UpdateSecretCommandHandler
{
    public class Handle_Should
    {
        [Fact]
        public async Task Execute_next_on_pipeline_when_no_errors_happens()
        {
            var mock = new Mock<IMediator>();
            var list = new List<Secret>()
            {
                new Secret
                {
                    SecretName = "pwd",
                    SecretValue = "123",
                    Owner = "user@mail.com"
                }
            };
            mock.Setup(o => o.Send(It.IsAny<UserSecretBySecretNameQuery>(), It.IsAny<CancellationToken>())).Returns<UserSecretBySecretNameQuery, CancellationToken>(async (query, cancelationToken) =>
            {
                return list.FirstOrDefault(f => f.SecretName == query.SecretName && f.Owner == query.Owner);
            });
            var handler = new VaultApplication.Secrets.Commands.UpdateSecretCommandHandler(mock.Object);
            var command = new VaultDomain.Commands.UpdateSecret.UpdateSecretCommand("user@mail.com", "pwd", "567");
            var result = await handler.Handle(command,
                new RequestHandlerDelegate<VaultDomain.ValueObjects.Result>(() =>
                {
                    return Task.FromResult(new VaultDomain.ValueObjects.Result().WithInfo("next step"));
                }),
                CancellationToken.None
            );
            Assert.Contains(result.Metadata.Infos, (p) => p == "next step");
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Return_error_secret_not_found_with_the_name_when_a_secret_with_the_name_not_exists()
        {
            var mock = new Mock<IMediator>();
            var handler = new VaultApplication.Secrets.Commands.UpdateSecretCommandHandler(mock.Object);
            var command = new VaultDomain.Commands.UpdateSecret.UpdateSecretCommand("user@mail.com", "pwd", "456546");
            var result = await handler.Handle(command,
                new RequestHandlerDelegate<VaultDomain.ValueObjects.Result>(() =>
                {
                    return Task.FromResult(new VaultDomain.ValueObjects.Result().WithInfo("next step"));
                }),
                CancellationToken.None
            );
            Assert.Contains(result.Metadata.Errors, (p) => p.ToLower().Contains("secret not found with the name"));
            Assert.False(result.Success);
        }
    }
}
