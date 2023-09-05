namespace VaultAppApi.Tests.Infrastructure
{
    public class Result
    {
        public bool Success { get => !Metadata.Errors.Any(); }
        public object Payload { get; set; }
        public Metadata Metadata { get; set; }
    }
}
