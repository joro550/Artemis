using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Artemis.Web.Server.Users;
using System.Collections.Generic;
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
        public async Task<List<Organization>> GetAllOrganizations([FromQuery] int? count, [FromQuery] int? offset)
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new GetOrganizations {Count = count ?? 50, Offset = offset ?? 0, UserId = user.User?.Id});
        }

        [HttpGet("{id}")]
        public async Task<Organization> GetOrganization(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new GetOrganizationById {Id = id, UserId = user.User?.Id});
        }

        [HttpGet("count")]
        public async Task<int> GetOrganizationCount()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new GetOrganizationCount { UserId = user.User?.Id });
        }

        [HttpGet("search/{name}")]
        public async Task<List<Organization>> SearchOrganizationNames(string name, [FromQuery] int? count, [FromQuery] int? offset)
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new SearchOrganizationName
            {
                SearchValue = name,
                Count = count ?? 50,
                Offset = offset ?? 0,
                UserId = user.User?.Id
            });
        }

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