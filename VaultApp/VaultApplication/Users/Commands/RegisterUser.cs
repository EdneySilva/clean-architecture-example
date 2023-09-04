using MediatR;
using VaultApplication.Services;
using VaultDomain.Commands.RegisterUser;
using VaultDomain.Queries.User;
using VaultDomain.ValueObjects;

namespace VaultApplication.Users.Commands
{
    public class RegisterUserCommandHandler : IPipelineBehavior<RegisterUserCommand, Result>
    {
        private readonly IMediator _mediator;

        public RegisterUserCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(RegisterUserCommand request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
        {
            try
            {
                var user = request.AsMaterializedUser();
                user.Password = SecretHasher.Hash(user.Password);
                user.CreatedAt = DateTime.UtcNow;
                var userWithSameLogin = await _mediator.Send(new UserByUserName(user.UserName));
                if (userWithSameLogin != null)
                    throw new Exception($"a user with same user name {user.UserName} is already registered");
                return await next();
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}
