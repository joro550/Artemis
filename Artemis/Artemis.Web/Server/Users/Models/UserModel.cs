using MediatR;
using System.Linq;
using System.Threading.Tasks;
using Artemis.Web.Shared.Events;
using Artemis.Web.Server.Employee;
using Artemis.Web.Shared.Organizations;

namespace Artemis.Web.Server.Users.Models
{
    public class UserModel
    {
        private readonly IMediator _mediator;

        public ApplicationUser User { get; }

        public UserModel(ApplicationUser user, IMediator mediator)
        {
            User = user;
            _mediator = mediator;
        }

        public async Task<bool> CanCreateEventFor(Organization organization) 
            => await IsEmployee(organization.Id);
        public async Task<bool> CanUpdateEventFor(Organization organization)
            => await IsEmployee(organization.Id);

        public async Task<bool> CanCreateUpdateFor(Event eventId) 
            => await IsEmployee(eventId.OrganizationId);

        public async Task<bool> CanCreateTemplatesFor(Organization organization)
            => await IsEmployee(organization.Id);

        private async Task<bool> IsEmployee(int organizationId)
        {
            var employees = await _mediator.Send(new GetEmployees {OrganizationId = organizationId});
            return employees.Any(emp => emp.UserId == User.Id);
        }
    }
}