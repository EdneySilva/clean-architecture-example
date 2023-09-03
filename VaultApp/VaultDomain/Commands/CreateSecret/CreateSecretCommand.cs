using FluentValidation;
using MediatR;
using VaultDomain.ValueObjects;

namespace VaultDomain.Commands.CreateSecret
{
    public class CreateSecretValidator : AbstractValidator<CreateSecretCommand>
    {
        public CreateSecretValidator()
        {
            RuleFor(v => v.Owner).NotEmpty().EmailAddress();
            RuleFor(v => v.Value).NotEmpty();
            RuleFor(v => v.CreatedAt).NotEmpty();
            RuleFor(v => v.Id).NotEmpty();
        }
    }

    public class CreateSecretCommand : IDomainCommand<Result>
    {
        private readonly Func<Entities.Secret> _secretProvider;

        public CreateSecretCommand(string owner, string name, string value)
        {
            _secretProvider = () => {
                var secret = new Entities.Secret();
                secret.Owner = owner;
                secret.SecretName = name;
                secret.SecretValue = value;
                secret.Id = Guid.NewGuid();
                secret.CreatedAt = DateTime.UtcNow;
                return secret;
            };
            Owner = owner;
            Name = name;
            Value = value;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public string Owner { get; }
        public string Name { get; }
        public string Value { get; }
        public DateTime CreatedAt { get; }

        public Entities.Secret Materialize() => _secretProvider();
    }
}
