using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Events;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Server.EventUpdates.Events;
using Artemis.Web.Server.Messaging.Adapters;
using Artemis.Web.Shared.MessageTemplates;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class OrganizationSubscriptionHandler
        :   INotificationHandler<EventCreatedNotification>,
            INotificationHandler<EventUpdateCreated>
    {
        private readonly ApplicationDbContext _context;
        private readonly MessagingClientAdapter _messageClient;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationSubscriptionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager, MessagingClientAdapter messageClient)
        {
            _context = context;
            _userManager = userManager;
            _messageClient = messageClient;
        }

        public async Task Handle(EventCreatedNotification notification, CancellationToken cancellationToken)
        {
            var messageTemplate = await _context.Set<MessageTemplateEntity>()
                .Where(entity => entity.OrganizationId == notification.Id && entity.MessageEvent == MessageEvent.EventCreated)
                .FirstOrDefaultAsync(cancellationToken);

            if (messageTemplate == null)
                return;

            var @event = await _context.Set<EventEntity>()
                .Where(entity => entity.Id == notification.Id)
                .SingleOrDefaultAsync(cancellationToken);

            var subscriptions = await _context.Set<OrganizationSubscriptionEntity>()
                .Where(entity => entity.OrganizationId == @event.OrganizationId)
                .ToListAsync(cancellationToken);

            Parallel.ForEach(subscriptions, async (sub) =>
            {
                var user = await _userManager.Users.Where(u => u.Id == sub.UserId)
                    .SingleOrDefaultAsync(cancellationToken);

                var result = await _messageClient.SendMessage(to: user.PhoneNumber,
                    message: messageTemplate.Text);

                await _context.Set<SentMessageEntity>()
                    .AddAsync(new SentMessageEntity {MessageId = result, UserId = user.Id}, cancellationToken);
            });
        }

        public Task Handle(EventUpdateCreated notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}