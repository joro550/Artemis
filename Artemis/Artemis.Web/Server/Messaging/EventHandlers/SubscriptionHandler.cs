using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Shared.MessageTemplates;
using Artemis.Web.Server.Messaging.Adapters;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class SubscriptionHandler
    {
        protected ApplicationDbContext Context { get; }

        private readonly MessagingClientAdapter _messageClient;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager, MessagingClientAdapter messageClient)
        {
            Context = context;
            _userManager = userManager;
            _messageClient = messageClient;
        }

        protected async Task SendMessagesForOrganization(int organizationId, MessageEvent messageEvent, CancellationToken cancellationToken)
        {
            var messageTemplate = await GetMessageTemplate(organizationId, messageEvent, cancellationToken);
            if (messageTemplate == null)
                return;

            var subscriptions = await GetSubscriptionsForEvent(organizationId, cancellationToken);
            if(subscriptions != null && subscriptions.Any())
                await SendMessagesToSubscriptions(subscriptions, messageTemplate, cancellationToken);
        }

        protected async Task SendMessagesFor(EventEntity eventEntity, MessageEvent messageEvent, CancellationToken cancellationToken)
        {
            var messageTemplate = await GetMessageTemplate(eventEntity.OrganizationId, messageEvent, cancellationToken);
            if (messageTemplate == null)
                return;

            var subscriptions = await GetSubscriptionsForEvent(eventEntity, cancellationToken);
            await SendMessagesToSubscriptions(subscriptions, messageTemplate, cancellationToken);
        }

        private async Task<MessageTemplateEntity> GetMessageTemplate(int organizationId, MessageEvent messageEvent, CancellationToken cancellationToken) =>
            await Context.Set<MessageTemplateEntity>()
                .Where(entity => entity.OrganizationId == organizationId && entity.MessageEvent == messageEvent)
                .FirstOrDefaultAsync(cancellationToken);

        private async Task<List<OrganizationSubscriptionEntity>> GetSubscriptionsForEvent(EventEntity eventEntity, CancellationToken cancellationToken) =>
            await Context.Set<OrganizationSubscriptionEntity>()
                .Where(entity => entity.OrganizationId == eventEntity.OrganizationId)
                .Distinct()
                .ToListAsync(cancellationToken);

        private async Task<List<OrganizationSubscriptionEntity>> GetSubscriptionsForEvent(int notificationId, CancellationToken cancellationToken)
        {
            var @event = await Context.Set<EventEntity>()
                .Where(entity => entity.Id == notificationId)
                .SingleOrDefaultAsync(cancellationToken);

            return await Context.Set<OrganizationSubscriptionEntity>()
                .Where(entity => entity.OrganizationId == @event.OrganizationId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        private async Task SendMessagesToSubscriptions(IEnumerable<OrganizationSubscriptionEntity> subscriptions,
            MessageTemplateEntity messageTemplate, CancellationToken cancellationToken)
        {
            foreach (var subscription in subscriptions)
            {
                var user = await _userManager.Users.Where(u => u.Id == subscription.UserId)
                    .SingleOrDefaultAsync(cancellationToken);
                
                var result = await _messageClient.SendMessage(to: user.PhoneNumber,
                    message: messageTemplate.Text);

                await Context.Set<SentMessageEntity>()
                    .AddAsync(new SentMessageEntity { MessageId = result, UserId = user.Id }, cancellationToken);
            }
        }
    }
}