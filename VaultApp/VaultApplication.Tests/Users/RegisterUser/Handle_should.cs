using MediatR;
using Moq;
using VaultDomain.Entities;
using VaultDomain.Queries.User;

namespace VaultApplication.Tests.Users.RegisterUser
{
    public class Handle_should
    {
        [Fact]
        public async Task Hash_the_password()
        {
            var mock = new Mock<IMediator>();
            var handler = new VaultApplication.Users.Commands.RegisterUserCommandHandler(mock.Object);
            var command = new VaultDomain.Commands.RegisterUser.RegisterUserCommand("user@mail.com", "password");
            await handler.Handle(command, 
                new RequestHandlerDelegate<VaultDomain.ValueObjects.Result>(() =>
                {
                    return Task.FromResult(new VaultDomain.ValueObjects.Result());
                }), 
                CancellationToken.None
            );
            var materializedUser = command.AsMaterializedUser();
            Assert.NotEqual(materializedUser.Password, "password");
        }

        [Fact]
        public async Task Set_CreatedAt()
        {
            var mock = new Mock<IMediator>();
            var handler = new VaultApplication.Users.Commands.RegisterUserCommandHandler(mock.Object);
            var initialDate = DateTime.UtcNow;
            var command = new VaultDomain.Commands.RegisterUser.RegisterUserCommand("user@mail.com", "password");
            await handler.Handle(command,
                new RequestHandlerDelegate<VaultDomain.ValueObjects.Result>(() =>
                {
                    return Task.FromResult(new VaultDomain.ValueObjects.Result());
                }),
                CancellationToken.None
            );
            var materializedUser = command.AsMaterializedUser();
            Assert.True(materializedUser.CreatedAt > initialDate);
        }

        [Fact]
        public async Task Pass_invoke_the_next_item_on_the_pipeline_when_all_roles_are_good()
        {
            var list = new List<User>();
            var mock = new Mock<IMediator>();
            var handler = new VaultApplication.Users.Commands.RegisterUserCommandHandler(mock.Object);
            var initialDate = DateTime.UtcNow;
            var command = new VaultDomain.Commands.RegisterUser.RegisterUserCommand("user@mail.com", "password");

            var result = await handler.Handle(command,
                new RequestHandlerDelegate<VaultDomain.ValueObjects.Result>(() =>
                {
                    return Task.FromResult(new VaultDomain.ValueObjects.Result().WithInfo("executed next"));
                }),
                CancellationToken.None
            );
            Assert.True(result.Success);
            Assert.Contains(result.Metadata.Infos, (p) => p == "executed next");
        }

        [Fact]
        public async Task Return_error_when_an_user_with_same_login_is_already_registered()
        {
            var list = new List<User>()
            {
                new User
                {
                    UserName = "user@mail.com",
                }
            };
            var mock = new Mock<IMediator>();
            mock.Setup(o => o.Send(It.IsAny<UserByUserName>(), It.IsAny<CancellationToken>())).Returns<UserByUserName, CancellationToken>(async (query, cancelationToken) =>
            {
                return list.FirstOrDefault(f => f.UserName == query.UserName);
            });
            var handler = new VaultApplication.Users.Commands.RegisterUserCommandHandler(mock.Object);
            var initialDate = DateTime.UtcNow;
            var command = new VaultDomain.Commands.RegisterUser.RegisterUserCommand("user@mail.com", "password");
            var result = await handler.Handle(command,
                new RequestHandlerDelegate<VaultDomain.ValueObjects.Result>(() =>
                {
                    return Task.FromResult(new VaultDomain.ValueObjects.Result());
                }),
                CancellationToken.None
            );
            Assert.False(result.Success);
            Assert.Contains(result.Metadata.Errors, (p) => p == "a user with same user name user@mail.com is already registered");
        }
    }
}
