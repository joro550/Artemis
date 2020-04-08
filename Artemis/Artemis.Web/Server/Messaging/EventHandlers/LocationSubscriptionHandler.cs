using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Events;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class LocationSubscriptionHandler
        : INotificationHandler<EventCreatedNotification>
    {
        public Task Handle(EventCreatedNotification notification, CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}