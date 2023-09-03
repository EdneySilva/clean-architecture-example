using VaultDomain.Entities;

namespace VaultDomain.Events
{
    public class SecretUpdated : BaseEvent
    {
        private readonly Secret _secret;
        public SecretUpdated(Secret secret)
        {
            _secret = secret;
        }
        public Secret Secret { get => _secret; }
    }
}
