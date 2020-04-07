using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Events;
using MediatR;
using Twilio.Rest.Api.V2010.Account;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class EventsHandler
        : INotificationHandler<EventCreatedNotification>
    {
        public Task Handle(EventCreatedNotification notification, CancellationToken cancellationToken)
        {
            var message = MessageResource.Create(
                body: "This is the ship that made the Kessel Run in fourteen parsecs?",
                from: new Twilio.Types.PhoneNumber("+15017122661"),
                to: new Twilio.Types.PhoneNumber("+15558675310")
            );

            return Task.CompletedTask;
        }
    }
}