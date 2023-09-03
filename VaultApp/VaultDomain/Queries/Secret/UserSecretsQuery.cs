using MediatR;
using VaultDomain.ValueObjects;

namespace VaultDomain.Queries.Secret
{
    public class UserSecretsQuery : IRequest<Result>
    {
        public UserSecretsQuery(string owner) 
        {
            Owner = owner;
        }

        public string Owner { get; }
    }
}
