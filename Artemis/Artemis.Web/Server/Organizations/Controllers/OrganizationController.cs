using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Artemis.Web.Server.Users;
using Microsoft.AspNetCore.Identity;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Shared.Organizations;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.Web.Server.Organizations.Controllers
{
    [ApiController]
    [Route("/api/organization")]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserAdapter _userManager;

        public OrganizationController(IMediator mediator, UserAdapter userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet("")]
        public async Task<List<Organization>> GetAllOrganizations()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new GetOrganizations {Count = 50, Offset = 0, UserId = user.User?.Id});
        }

        [HttpGet("{id}")]
        public async Task<Organization> GetOrganization(int id)
            => await _mediator.Send(new GetOrganizationById {Id = id});

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrganization(CreateOrganization org)
        {
            var user = await _userManager.GetUserAsync(User);
            await _mediator.Publish(new CreateOrganizationNotification {Organization = org, UserId = user.User?.Id});
            return Ok();
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOrganization(EditOrganization org)
        {
            var organization = await _mediator.Send(new GetOrganizationById { Id = org.Id});
            if (organization == null)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            await _mediator.Publish(new EditOrganizationNotification { Organization = org, UserId = user.User?.Id });
            return Ok();
        }
    }
}