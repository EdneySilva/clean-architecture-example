using MediatR;
using VaultApplication.Services;
using VaultDomain.Commands.AuthenticateUser;
using VaultDomain.Queries.User;
using VaultDomain.ValueObjects;

namespace VaultApplication.Users.Commands
{
    internal class AuthenticationUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Result>
    {
        private readonly IMediator _mediator;

        public AuthenticationUserCommandHandler(IMediator mediator)
		{
            _mediator = mediator;
        }

        public async Task<Result> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
			try
			{
				var user = await _mediator.Send(new UserByUserName(request.UserName));
                if (user == null)
                    return new Result().WithError("Invalid user or password");
                if(!SecretHasher.Verify(request.Password, user.Password))
                    return new Result().WithError("Invalid user or password");
                user.Password = string.Empty;
                return new Result(user);
            }
			catch (Exception ex)
			{
				return new Result(ex);
			}
        }
    }
}
