using Artemis.Web.Shared.Events;
using MediatR;

namespace Artemis.Web.Server.Events
{
    public class CreateEventNotification : INotification
    {
        public CreateEvent Event { get; set; }
    }
}