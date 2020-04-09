using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.EventUpdates.Events;
using Artemis.Web.Server.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class EventSubscriptionHandler
        :   INotificationHandler<EventUpdateCreated>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventSubscriptionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task Handle(EventUpdateCreated notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}