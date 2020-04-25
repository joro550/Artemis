using MediatR;

namespace Artemis.Web.Server.Events
{
    public class EventUpdatedNotification : INotification
    {
        public int Id { get; set; }
    }
}