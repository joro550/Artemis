using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Artemis.Web.Server.Events;
using Artemis.Web.Shared.Events;
using System.Collections.Generic;
using Artemis.Web.Server.Organizations;
using Artemis.Web.Server.Users;
using Artemis.Web.Server.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CreateEvent = Artemis.Web.Shared.Events.CreateEvent;

namespace Artemis.Web.Server.Controllers
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