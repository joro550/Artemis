using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Artemis.Web.Server.MessageTemplates;
using Artemis.Web.Shared.MessageTemplates;

namespace Artemis.Web.Server.Controllers
{
    [ApiController]
    [Route("api/organization/{organizationId:int}/template")]
    public class MessageTemplateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageTemplateController(IMediator mediator) 
            => _mediator = mediator;

        [HttpGet]
        public async Task<List<MessageTemplate>> GetOrganizationTemplates(int organizationId) =>
            await _mediator.Send(new GetMessageTemplates{Count = 50, Offset = 0, OrganizationId = organizationId});

        [HttpGet("{templateId:int}")]
        public async Task<MessageTemplate> GetMessageTemplate(int organizationId, int templateId) 
            => await _mediator.Send(new GetMessageTemplate {Id = templateId, OrganizationId = organizationId});

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTemplate(CreateMessageTemplate model)
        {
            await _mediator.Publish(new CreateMessageTemplateNotification {Model = model});
            return Ok();
        }
    }
}