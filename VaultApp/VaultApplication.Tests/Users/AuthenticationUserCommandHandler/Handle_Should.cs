using MediatR;
using Moq;
using VaultApplication.Services;
using VaultDomain.Entities;
using VaultDomain.Queries.User;

namespace VaultApplication.Tests.Users.AuthenticationUserCommandHandler
{
    public class Handle_should
    {
        [Fact]
        public async Task Return_error_Invalid_user_or_password_when_not_found_an_user()
        {
            var mock = new Mock<IMediator>();
            var handler = new VaultApplication.Users.Commands.AuthenticationUserCommandHandler(mock.Object);
            var command = new VaultDomain.Commands.AuthenticateUser.AuthenticateUserCommand("user@mail.com", "password");
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.False(result.Success);
            Assert.Contains(result.Metadata.Errors, (p) => p == "Invalid user or password");
        }

        [Fact]
        public async Task Return_error_Invalid_user_or_password_when_password_does_not_match()
        {
            var pwd = SecretHasher.Hash("123456");
            var list = new List<User>()
            {
                new User
                {
                    UserName = "user@mail.com",
                    Password = pwd
                }
            };
            var mock = new Mock<IMediator>();
            mock.Setup(o => o.Send(It.IsAny<UserByUserName>(), It.IsAny<CancellationToken>())).Returns<UserByUserName, CancellationToken>(async (query, cancelationToken) =>
            {
                return list.FirstOrDefault(f => f.UserName == query.UserName);
            });
            var handler = new VaultApplication.Users.Commands.AuthenticationUserCommandHandler(mock.Object);
            var command = new VaultDomain.Commands.AuthenticateUser.AuthenticateUserCommand("user@mail.com", "password");
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.False(result.Success);
            Assert.Contains(result.Metadata.Errors, (p) => p == "Invalid user or password");
        }

        [Fact]
        public async Task Clear_User_Password_on_success_result()
        {
            var pwd = SecretHasher.Hash("password");
            var list = new List<User>()
            {
                new User
                {
                    UserName = "user@mail.com",
                    Password = pwd
                }
            };
            var mock = new Mock<IMediator>();
            mock.Setup(o => o.Send(It.IsAny<UserByUserName>(), It.IsAny<CancellationToken>())).Returns<UserByUserName, CancellationToken>(async (query, cancelationToken) =>
            {
                return list.FirstOrDefault(f => f.UserName == query.UserName);
            });
            var handler = new VaultApplication.Users.Commands.AuthenticationUserCommandHandler(mock.Object);
            var command = new VaultDomain.Commands.AuthenticateUser.AuthenticateUserCommand("user@mail.com", "password");
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.True(result.Success);
            Assert.Empty(((User)result.Payload).Password);
        }

        [Fact]
        public async Task Retur_success_when_found_user_and_match_password()
        {
            var pwd = SecretHasher.Hash("password");
            var list = new List<User>()
            {
                new User
                {
                    UserName = "user@mail.com",
                    Password = pwd
                }
            };
            var mock = new Mock<IMediator>();
            mock.Setup(o => o.Send(It.IsAny<UserByUserName>(), It.IsAny<CancellationToken>())).Returns<UserByUserName, CancellationToken>(async (query, cancelationToken) =>
            {
                return list.FirstOrDefault(f => f.UserName == query.UserName);
            });
            var handler = new VaultApplication.Users.Commands.AuthenticationUserCommandHandler(mock.Object);
            var command = new VaultDomain.Commands.AuthenticateUser.AuthenticateUserCommand("user@mail.com", "password");
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.True(result.Success);
        }
    }
}
