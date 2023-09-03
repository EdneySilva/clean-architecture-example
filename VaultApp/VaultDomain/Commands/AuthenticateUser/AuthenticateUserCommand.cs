using FluentValidation;
using MediatR;
using VaultDomain.ValueObjects;

namespace VaultDomain.Commands.AuthenticateUser
{
    public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(v => v.UserName).NotEmpty().EmailAddress();
            RuleFor(v => v.Password).NotEmpty().MinimumLength(8);
        }
    }

    public class AuthenticateUserCommand : IDomainCommand<Result>
    {
        public AuthenticateUserCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; }
        public string Password { get; }
    }
}
