using MediatR;
using System.Threading.Tasks;
using Artemis.Web.Server.Users;
using Microsoft.AspNetCore.Mvc;
using Artemis.Web.Server.Events;
using System.Collections.Generic;
using Artemis.Web.Shared.EventUpdates;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.Web.Server.EventUpdates.Controllers
{
    [ApiController]
    [Route("/api/organization/{organizationId}/event/{eventId}/update")]
    public class EventUpdateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserAdapter _userManager;

        public EventUpdateController(IMediator mediator, UserAdapter userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<List<EventUpdate>> GetEventUpdates(int organizationId, int eventId, [FromQuery] int? count, [FromQuery] int? offset) =>
            await _mediator.Send(new GetEventUpdates {OrganizationId = organizationId, EventId = eventId, Count = count ?? 50, Offset = offset ?? 0});

        [HttpGet("{updateId:int}")]
        public async Task<EventUpdate> GetEventUpdate(int organizationId, int eventId, int updateId) 
            => await _mediator.Send(new GetEventUpdate {OrganizationId = organizationId, EventId = eventId, UpdateId = updateId});

        [HttpGet("count")]
        public async Task<int> GetEventUpdateCount(int eventId) 
            => await _mediator.Send(new GetEventUpdateCount {EventId = eventId});

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEventUpdate(CreateEventUpdate model)
        {
            var @event = await _mediator.Send(new GetEvent {OrganizationId = model.OrganizationId, Id = model.EventId});
            if (@event == null)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            var canCreateEvent = await user.CanCreateUpdateFor(@event);

            if (!canCreateEvent)
                return Unauthorized();

            await _mediator.Publish(new CreateEventUpdateNotification {Model = model});
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateEventUpdate(EditEventUpdate model)
        {
            var @event = await _mediator.Send(new GetEvent { OrganizationId = model.OrganizationId, Id = model.EventId });
            if (@event == null)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            var canCreateEvent = await user.CanCreateUpdateFor(@event);
            if (!canCreateEvent)
                return Unauthorized();

            await _mediator.Publish(new EditEventUpdateNotification { Model = model });
            return Ok();
        }
    }
}
