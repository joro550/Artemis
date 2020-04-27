using MediatR;
using System.Threading.Tasks;
using Artemis.Web.Server.Users;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Artemis.Web.Server.Organizations;
using Artemis.Web.Shared.MessageTemplates;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.Web.Server.MessageTemplates.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/organization/{organizationId:int}/template")]
    public class MessageTemplateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserAdapter _userManager;

        public MessageTemplateController(IMediator mediator, IUserAdapter userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<List<MessageTemplate>> GetOrganizationTemplates(int organizationId, [FromQuery] int? count, [FromQuery] int? offset) =>
            await _mediator.Send(new GetMessageTemplates{Count = count ?? 50, Offset = offset ?? 0, OrganizationId = organizationId});

        [HttpGet("{templateId:int}")]
        public async Task<MessageTemplate> GetMessageTemplate(int organizationId, int templateId) 
            => await _mediator.Send(new GetMessageTemplate {Id = templateId, OrganizationId = organizationId});

        [HttpGet("count")]
        public async Task<int> GetMessageTemplateCount(int organizationId) 
            => await _mediator.Send(new GetMessageTemplateCount {OrganizationId = organizationId});

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTemplate(CreateMessageTemplate model)
        {
            var organization = await _mediator.Send(new GetOrganizationById { Id = model.OrganizationId });
            if (organization == null)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            var canCreateTemplate = await user.CanCreateTemplatesFor(organization);

            if (!canCreateTemplate)
                return Unauthorized();

            await _mediator.Publish(new CreateMessageTemplateNotification {Model = model});
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateTemplate(EditMessageTemplate model)
        {
            var organization = await _mediator.Send(new GetOrganizationById { Id = model.OrganizationId });
            if (organization == null)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            var canCreateTemplate = await user.CanCreateTemplatesFor(organization);

            if (!canCreateTemplate)
                return Unauthorized();

            await _mediator.Publish(new EditMessageTemplateNotification { Model = model });
            return Ok();
        }
    }
}