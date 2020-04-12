using System.Collections.Generic;
using System.Threading.Tasks;
using Artemis.Web.Server.Organizations;
using Artemis.Web.Server.Users;
using Artemis.Web.Shared.MessageTemplates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Web.Server.MessageTemplates.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/organization/{organizationId:int}/template")]
    public class MessageTemplateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserAdapter _userManager;

        public MessageTemplateController(IMediator mediator, UserAdapter userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<List<MessageTemplate>> GetOrganizationTemplates(int organizationId) =>
            await _mediator.Send(new GetMessageTemplates{Count = 50, Offset = 0, OrganizationId = organizationId});

        [HttpGet("{templateId:int}")]
        public async Task<MessageTemplate> GetMessageTemplate(int organizationId, int templateId) 
            => await _mediator.Send(new GetMessageTemplate {Id = templateId, OrganizationId = organizationId});

        [HttpPost]
        // [ValidateAntiForgeryToken]
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
    }
}