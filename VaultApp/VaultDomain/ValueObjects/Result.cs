namespace VaultDomain.ValueObjects
{
    public struct Result
    {
        public bool Success { get => !Metadata.Errors.Any(); }
        public object Payload { get; set; }
        public Metadata Metadata { get; }

        public Result()
        {
            Metadata = new Metadata();
        }

        public Result(object payload)
        {
            Payload = payload;
            Metadata = new Metadata();
        }

        public Result(IEnumerable<string> notes)
        {
            Payload = null;
            Metadata = new Metadata();
            foreach (var note in notes)
                Metadata.AnotateInfo(note);
        }

        public Result(Exception ex)
        {
            Payload = null;
            Metadata = new Metadata();
            Metadata.AnotateError(ex.Message);
            if(ex.StackTrace != null)
                Metadata.AnotateInfo(ex.StackTrace);
        }

        public Result WithInfo(string info)
        {
            Metadata.AnotateInfo(info);
            return this;
        }

        public Result WithWarning(string warning)
        {
            Metadata.AnotateWarning(warning);
            return this;
        }

        public Result WithError(string error)
        {
            Metadata.AnotateError(error);
            return this;
        }
    }

    public struct Metadata
    {
        private readonly List<string> _infos;
        private readonly List<string> _warnings;
        private readonly List<string> _errors;
        public IEnumerable<string> Infos { get => _infos; }
        public IEnumerable<string> Warnings { get => _warnings; }
        public IEnumerable<string> Errors { get => _errors; }

        public Metadata()
        {
            _infos = new();
            _warnings = new();
            _errors = new();
        }

        internal void AnotateInfo(string info) => _infos.Add(info);
        internal void AnotateWarning(string warning) => _warnings.Add(warning);
        internal void AnotateError(string error) => _errors.Add(error);
    }
}
