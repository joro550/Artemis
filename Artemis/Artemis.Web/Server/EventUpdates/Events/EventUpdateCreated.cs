using MediatR;

namespace Artemis.Web.Server.EventUpdates.Events
{
    public class EventUpdateCreated : INotification
    {
        public int Id { get; set; }
    }
}