using MediatR;
using System.Linq;
using System.Threading.Tasks;
using Artemis.Web.Shared.Organizations;

namespace Artemis.Web.Server.Employee
{
    public class ThingThing
    {
        private readonly IMediator _mediator;

        public ThingThing(IMediator mediator) 
            => _mediator = mediator;

        public async Task<bool> CanCreateEventFor(string userId, Organization organization)
        {
            var employees = await _mediator.Send(new GetEmployees {OrganizationId = organization.Id});
            return employees.Any(x => x.UserId == userId);
        }
    }
}