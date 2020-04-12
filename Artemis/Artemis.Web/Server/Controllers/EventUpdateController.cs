using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Artemis.Web.Server.Events;
using Artemis.Web.Server.EventUpdates;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Shared.EventUpdates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Web.Server.Controllers
{
    [ApiController]
    [Route("api/organization/{organizationId:int}/event/{eventId:int}/update")]
    public class EventUpdateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventUpdateController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<List<EventUpdate>> GetEventUpdates(int organizationId, int eventId)
        {
            return await _mediator.Send(new GetEventUpdates
                {OrganizationId = organizationId, EventId = eventId, Count = 100, Offset = 0});
        }

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
    }
}
