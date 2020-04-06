using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Artemis.Web.Server.Organizations;
using Artemis.Web.Shared.Organizations;

namespace Artemis.Web.Server.Controllers
{
    [ApiController]
    [Route("/api/Organization")]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrganizationController(IMediator mediator) 
            => _mediator = mediator;

        [HttpGet("")]
        public async Task<List<Organization>> GetAllOrganizations() 
            => await _mediator.Send(new GetOrganizations{Offset = 0, Count = 50});

        [HttpGet("{id}")]
        public async Task<Organization> GetOrganization(int id)
            => await _mediator.Send(new GetOrganizationById {Id = id});

        [HttpPost]
        public async Task<IActionResult> CreateOrganization(CreateOrganization org)
        {
            await _mediator.Publish(new CreateOrganizationNotification {Name = org.Name});
            return Ok();
        }
    }
}