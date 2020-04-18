
namespace Artemis.Web.Shared.EventUpdates
{
    public class EventUpdate
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string CallToAction { get; set; }
    }
}