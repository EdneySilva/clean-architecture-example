namespace VaultDomain
{
    public class BaseEntity
    {
        private List<BaseEvent> _events = new List<BaseEvent>();

        public DateTime CreatedAt { get; set; }

        protected void AddEvent(BaseEvent baseEvent)
        {
            _events.Add(baseEvent);
        }

        public IEnumerable<BaseEvent> TakeEvents()
        {
            var events = _events.ToArray();
            _events.Clear();
            return events;
        }
    }
}
