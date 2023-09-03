using FluentValidation;
using VaultDomain.ValueObjects;

namespace VaultDomain.Commands.DeleteSecret
{
    public class DeleteSecretValidator : AbstractValidator<DeleteSecretCommand>
    {
        public DeleteSecretValidator()
        {
            RuleFor(v => v.Owner).NotEmpty().EmailAddress();
            RuleFor(v => v.Name).NotEmpty();
        }
    }

    public class DeleteSecretCommand : IDomainCommand<Result>
    {
        private readonly Func<Entities.Secret> _secretProvider;

        public DeleteSecretCommand(string owner, string name)
        {
            _secretProvider = () =>
            {
                var secret = new Entities.Secret(Id, Name!, Owner!);
                secret.AddEvent(new Events.SecretDeleted(secret));
                return secret;
            };
            Owner = owner;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Owner { get; }
        public string Name { get; }

        public Entities.Secret Materialize() => _secretProvider();
    }
}
