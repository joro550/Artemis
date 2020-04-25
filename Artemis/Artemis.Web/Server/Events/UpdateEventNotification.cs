using Artemis.Web.Shared.Events;
using MediatR;

namespace Artemis.Web.Server.Events
{
    public class UpdateEventNotification : INotification
    {
        public EditEvent Event { get; set; }
    }
}