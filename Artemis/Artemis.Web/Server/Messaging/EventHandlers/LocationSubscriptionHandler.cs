using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Events;
using Artemis.Web.Server.EventUpdates.Events;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class LocationSubscriptionHandler
        :   INotificationHandler<EventCreatedNotification>,
            INotificationHandler<EventUpdateCreated>
    {
        public Task Handle(EventCreatedNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(EventUpdateCreated notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}