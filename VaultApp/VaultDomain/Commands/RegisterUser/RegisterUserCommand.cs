using FluentValidation;
using VaultDomain.Entities;
using VaultDomain.ValueObjects;

namespace VaultDomain.Commands.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(v => v.UserName).NotEmpty().EmailAddress();
            RuleFor(v => v.Password).NotEmpty().MinimumLength(8);
        }
    }

    public class RegisterUserCommand : IDomainCommand<Result>
    {
        private User _user;

        public RegisterUserCommand(string userName, string password)
        {
            _user = new User();
            UserName = _user.UserName = userName;
            Password = _user.Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        public User AsMaterializedUser()
        {
            return _user;
        }
    }
}
