using Artemis.Web.Server.Data;
using Artemis.Web.Server.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Web.Server.Messaging.EventHandlers
{
    public class EventSubscriptionHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventSubscriptionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
    }
}