using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Server.Organizations;
using Artemis.Web.Shared.Organizations;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.Web.Server.Controllers
{
    [ApiController]
    [Route("/api/Organization")]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet("")]
        public async Task<List<Organization>> GetAllOrganizations() 
            => await _mediator.Send(new GetOrganizations { Count = 50, Offset = 0 });

        [HttpGet("{id}")]
        public async Task<Organization> GetOrganization(int id)
            => await _mediator.Send(new GetOrganizationById {Id = id});

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrganization(CreateOrganization org)
        {
            var user = await _userManager.GetUserAsync(User);
            await _mediator.Publish(new CreateOrganizationNotification {Organization = org, UserId = user.Id});
            return Ok();
        }
    }
}