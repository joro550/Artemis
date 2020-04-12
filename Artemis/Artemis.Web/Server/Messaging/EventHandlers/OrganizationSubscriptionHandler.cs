using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Events;
using Microsoft.AspNetCore.Identity;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Shared.MessageTemplates;
using Artemis.Web.Server.Messaging.Adapters;
using Artemis.Web.Server.EventUpdates.Events;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class OrganizationSubscriptionHandler 
        :   SubscriptionHandler,
            INotificationHandler<EventUpdateCreated>,
            INotificationHandler<EventCreatedNotification>
    {
        public OrganizationSubscriptionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager, MessagingClientAdapter messageClient)
            : base(context, userManager, messageClient)
        {
        }

        public async Task Handle(EventCreatedNotification notification, CancellationToken cancellationToken) 
            => await SendMessagesForOrganization(notification.Id, MessageEvent.EventCreated, cancellationToken);

        public async Task Handle(EventUpdateCreated notification, CancellationToken cancellationToken) 
            => await SendMessagesForOrganization(notification.Id, MessageEvent.EventUpdateCreated, cancellationToken);

    }
}