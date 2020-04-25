using MediatR;

namespace Artemis.Web.Server.Events
{
    public class EventCreatedNotification : INotification
    {
        public int Id { get; set; }
    }
}