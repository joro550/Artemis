using Artemis.Web.Shared.EventUpdates;
using MediatR;

namespace Artemis.Web.Server.EventUpdates
{
    public class CreateEventUpdateNotification : INotification
    {
        public CreateEventUpdate Model { get; set; }
    }
}