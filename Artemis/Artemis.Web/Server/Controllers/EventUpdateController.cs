using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Artemis.Web.Server.EventUpdates;
using Artemis.Web.Shared.EventUpdates;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.Web.Server.Controllers
{
    [ApiController]
    [Route("api/organization/{organizationId:int}/event/{eventId:int}/update")]
    public class EventUpdateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventUpdateController(IMediator mediator) 
            => _mediator = mediator;

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
            await _mediator.Publish(new CreateEventUpdateNotification {Model = model});
            return Ok();
        }
    }
}
