using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Server.Events;
using Artemis.Web.Server.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Twilio.Rest.Api.V2010.Account;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class OrganizationSubscriptionHandler
        : INotificationHandler<EventCreatedNotification>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationSubscriptionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Handle(EventCreatedNotification notification, CancellationToken cancellationToken)
        {
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
                
                var message = await MessageResource.CreateAsync(
                    body: "This is the ship that made the Kessel Run in fourteen parsecs?",
                    @from: new Twilio.Types.PhoneNumber("+15017122661"),
                    to: new Twilio.Types.PhoneNumber(user.PhoneNumber)
                );

                await _context.Set<SentMessageEntity>()
                    .AddAsync(new SentMessageEntity {MessageId = message.Sid, UserId = user.Id}, cancellationToken);
            });
        }
    }
}