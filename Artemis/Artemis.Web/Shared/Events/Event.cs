namespace Artemis.Web.Shared.Events
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPublished { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public EventType EventType { get; set; }
    }
}