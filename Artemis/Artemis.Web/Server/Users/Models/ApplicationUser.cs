using System.Threading.Tasks;
using Artemis.Web.Shared.Events;
using Artemis.Web.Shared.Organizations;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Web.Server.Users.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            
        }

        public Task<bool> CanCreateEventFor(Organization organization)
        {
            return Task.FromResult(true);
        }

        public Task<bool> CanCreateUpdateFor(Event eventId)
        {
            return Task.FromResult(true);
        }
    }
}
