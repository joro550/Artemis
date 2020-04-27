using MediatR;
using System.Threading.Tasks;
using Artemis.Web.Server.Users;
using Microsoft.AspNetCore.Mvc;
using Artemis.Web.Shared.Events;
using System.Collections.Generic;
using Artemis.Web.Server.Organizations;
using Microsoft.AspNetCore.Authorization;
using CreateEvent = Artemis.Web.Shared.Events.CreateEvent;

namespace Artemis.Web.Server.Events.Controllers
{
    [ApiController]
    [Route("/api/organization/{organizationId}/Event")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserAdapter _userManager;

        public EventsController(IMediator mediator, IUserAdapter userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<List<Event>> GetEventsForOrganization(int organizationId, [FromQuery] int? count, [FromQuery] int? offset)
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new GetEvents {OrganizationId = organizationId, UserId = user.User?.Id, Offset = offset ?? 0, Count = count ?? 50 });
        }

        [HttpGet("{id}")]
        public async Task<Event> GetEvent(int organizationId, int id)
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new GetEvent {Id = id, UserId = user.User?.Id, OrganizationId = organizationId});
        }

        [HttpGet("count")]
        public async Task<int> GetEventCount(int organizationId)
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new GetEventCount { UserId = user.User?.Id, OrganizationId = organizationId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent(CreateEvent model)
        {
            var user = await _userManager.GetUserAsync(User);
            var organization = await _mediator.Send(new GetOrganizationById {UserId = user.User?.Id, Id = model.OrganizationId});
            if (organization == null)
                return BadRequest();

            var canCreateEvent = await user.CanCreateEventFor(organization);

            if (!canCreateEvent)
                return Unauthorized();

            await _mediator.Publish(new CreateEventNotification {Event = model});
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(EditEvent model)
        {
            var user = await _userManager.GetUserAsync(User);
            var organization = await _mediator.Send(new GetOrganizationById { UserId = user.User?.Id, Id = model.OrganizationId });
            if (organization == null)
                return BadRequest();

            var canCreateEvent = await user.CanUpdateEventFor(organization);

            if (!canCreateEvent)
                return Unauthorized();

            await _mediator.Publish(new UpdateEventNotification {Event = model});
            return Ok();
        }
    }
}