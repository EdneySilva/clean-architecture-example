namespace VaultDomain.Entities
{
    public class Secret : BaseEntity
    {
        public Guid Id { get; set; }
        public string SecretName { get; set; }
        public string SecretValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Owner { get; set; }
    }
}
