using VaultDomain.Entities;

namespace VaultDomain.Events
{
    public class SecretDeleted : BaseEvent
    {
        private readonly Secret _secret;
        public SecretDeleted(Secret secret)
        {
            _secret = secret;
        }
        public Secret Secret { get => _secret; }
    }
}
