using VaultDomain.Entities;

namespace VaultDomain.Events
{
    public class SecretCreated : BaseEvent
    {
        private readonly Secret _secret;
        public SecretCreated(Secret secret)
        {
            _secret = secret;
        }
        public Secret Secret { get => _secret; }
    }
}
