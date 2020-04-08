using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Artemis.Web.Server.Events;
using Artemis.Web.Shared.Events;
using System.Collections.Generic;
using CreateEvent = Artemis.Web.Shared.Events.CreateEvent;

namespace Artemis.Web.Server.Controllers
{
    [ApiController]
    [Route("/api/organization/{organizationId}/Event")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventsController(IMediator mediator) 
            => _mediator = mediator;

        [HttpGet]
        public async Task<List<Event>> GetEventsForOrganization(int organizationId)
            => await _mediator.Send(new GetEvents {OrganizationId = organizationId, Offset = 0, Count = 50});


        [HttpGet("{id}")]
        public async Task<Event> GetEvent(int organizationId, int id)
            => await _mediator.Send(new GetEvent {Id = id, OrganizationId = organizationId});

        [HttpPost]
        public async Task<IActionResult> CreateEvent(int organizationId, CreateEvent model)
        {
            model.OrganizationId = organizationId;
            await _mediator.Publish(new CreateEventNotification {Event = model});
            return Ok();
        }
    }
}