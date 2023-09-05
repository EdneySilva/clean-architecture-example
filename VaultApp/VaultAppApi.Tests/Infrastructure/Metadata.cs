namespace VaultAppApi.Tests.Infrastructure
{
    public class Metadata
    {
        public IEnumerable<string> Infos { get; set; }
        public IEnumerable<string> Warnings { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
