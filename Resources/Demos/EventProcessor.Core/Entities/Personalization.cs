namespace EventProcessor.Core.Entities
{
    public class Personalization
    {
        public EventHeader Header { get; set; }
        public string PersonalizationType { get; set; }
        public string Data { get; set; }
    }
}
