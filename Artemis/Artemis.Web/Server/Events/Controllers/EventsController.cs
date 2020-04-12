using System.Collections.Generic;
using System.Threading.Tasks;
using Artemis.Web.Server.Organizations;
using Artemis.Web.Server.Users;
using Artemis.Web.Shared.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CreateEvent = Artemis.Web.Shared.Events.CreateEvent;

namespace Artemis.Web.Server.Events.Controllers
{
    [ApiController]
    [Route("/api/organization/{organizationId}/Event")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserAdapter _userManager;

        public EventsController(IMediator mediator, UserAdapter userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<List<Event>> GetEventsForOrganization(int organizationId)
            => await _mediator.Send(new GetEvents {OrganizationId = organizationId, Offset = 0, Count = 50});


        [HttpGet("{id}")]
        public async Task<Event> GetEvent(int organizationId, int id)
            => await _mediator.Send(new GetEvent {Id = id, OrganizationId = organizationId});

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent(CreateEvent model)
        {
            var organization = await _mediator.Send(new GetOrganizationById { Id = model.OrganizationId});
            if (organization == null)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            var canCreateEvent = await user.CanCreateEventFor(organization);

            if (!canCreateEvent)
                return Unauthorized();

            await _mediator.Publish(new CreateEventNotification {Event = model});
            return Ok();
        }
    }
}