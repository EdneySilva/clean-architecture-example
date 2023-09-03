using MediatR;

namespace VaultDomain.Queries.Secret
{
    public class UserSecretBySecretNameQuery : IRequest<Entities.Secret>
    {
        public UserSecretBySecretNameQuery(string owner, string secretName)
        {
            Owner = owner;
            SecretName = secretName;
        }

        public string Owner { get; }
        public string SecretName { get; }
    }
}
