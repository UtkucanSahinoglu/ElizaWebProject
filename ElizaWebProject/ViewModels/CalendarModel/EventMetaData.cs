namespace ElizaWebProject.Models
{
    public class EventMetaData
    {
        public string Memo { get; set; }
        public bool HasToOrCc { get; set; }
        public bool HasRecurrenceRule { get; set; }
        public string Location { get; set; }
        public EventCreateInfo Creator { get; set; }
    }
}
