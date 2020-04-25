using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Data.Models;
using MediatR;

namespace Artemis.Web.Server.Subscriptions.EventHandlers
{
    public class SubscriptionHandler
        :   INotificationHandler<CreateLocationSubscription>,
            INotificationHandler<CreateEventSubscription>,
            INotificationHandler<CreateOrganizationSubscription>
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionHandler(ApplicationDbContext context) 
            => _context = context;

        public async Task Handle(CreateLocationSubscription notification, CancellationToken cancellationToken)
        {
            await _context.Subscriptions.AddAsync(new LocationSubscriptionEntity
            {
                UserId = notification.UserId,
                OrganizationId = notification.OrganizationId
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(CreateEventSubscription notification, CancellationToken cancellationToken)
        {
            await _context.Subscriptions.AddAsync(new EventSubscriptionEntity
            {
                UserId = notification.UserId,
                EventId = notification.EventId
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(CreateOrganizationSubscription notification, CancellationToken cancellationToken)
        {
            await _context.Subscriptions.AddAsync(new OrganizationSubscriptionEntity()
            {
                UserId = notification.UserId,
                OrganizationId = notification.OrganizationId
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}