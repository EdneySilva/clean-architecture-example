using VaultDomain.Events;

namespace VaultDomain.Entities
{
    public class Secret : BaseEntity
    {
        public Secret()
        {
            AddEvent(new SecretCreated(this));
        }

        public Secret(Guid id)
        {
            Id = id;
            AddEvent(new SecretUpdated(this));
        }
        
        public Secret(Guid id, string name, string owner)
        {
            Id = id;
            SecretName = name;
            Owner = owner;
        }

        public Guid Id { get; set; }
        public string SecretName { get; set; }
        public string SecretValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Owner { get; set; }
    }
}
