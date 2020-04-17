using Artemis.Web.Shared.EventUpdates;
using MediatR;

namespace Artemis.Web.Server.EventUpdates
{
    public class EditEventUpdateNotification : INotification
    {
        public EditEventUpdate Model { get; set; }
    }
}