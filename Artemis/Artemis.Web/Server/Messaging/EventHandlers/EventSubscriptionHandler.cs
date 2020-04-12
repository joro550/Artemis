using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Shared.MessageTemplates;
using Artemis.Web.Server.EventUpdates.Events;
using Artemis.Web.Server.Messaging.Adapters;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class EventSubscriptionHandler
        :   SubscriptionHandler,
            INotificationHandler<EventUpdateCreated>
    {
        public EventSubscriptionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager, MessagingClientAdapter messageClient) 
            : base(context, userManager, messageClient)
        {
        }

        public async Task Handle(EventUpdateCreated notification, CancellationToken cancellationToken)
        {
            var eventUpdateEntity = await Context.Set<EventUpdateEntity>()
                .FirstOrDefaultAsync(entity => entity.Id == notification.Id, cancellationToken);

            var eventEntity = await Context.Set<EventEntity>()
                .FirstOrDefaultAsync(entity => entity.Id == eventUpdateEntity.EventId, cancellationToken);

            await SendMessagesFor(eventEntity, MessageEvent.EventUpdateCreated, cancellationToken);
        }
    }
}