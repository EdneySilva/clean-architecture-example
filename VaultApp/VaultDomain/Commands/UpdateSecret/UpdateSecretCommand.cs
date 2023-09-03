using FluentValidation;
using VaultDomain.ValueObjects;

namespace VaultDomain.Commands.UpdateSecret
{
    public class UpdateSecretValidator : AbstractValidator<UpdateSecretCommand>
    {
        public UpdateSecretValidator()
        {
            RuleFor(v => v.Owner).NotEmpty().EmailAddress();
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.Value).NotEmpty();
        }
    }

    public class UpdateSecretCommand : IDomainCommand<Result>
    {
        private readonly Func<Entities.Secret> _secretProvider;

        public UpdateSecretCommand(string owner, string name, string value)
        {
            _secretProvider = () => {
                var secret = new Entities.Secret(this.Id);
                secret.Owner = this.Owner;
                secret.SecretName = this.Name;
                secret.SecretValue = this.Value;
                return secret;
            };
            Owner = owner;
            Name = name;
            Value = value;
        }

        public Guid Id { get; set; }
        public string Owner { get; }
        public string Name { get; }
        public string Value { get; }

        public Entities.Secret Materialize() => _secretProvider();
    }
}   
