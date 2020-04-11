using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Artemis.Web.Shared.Employee;
using Artemis.Web.Server.Employee;
using Microsoft.AspNetCore.Identity;
using Artemis.Web.Server.Users.Models;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.Web.Server.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("status")]
        public async Task<List<EmployeeStatusResponse>> GetEmployeeStatus()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new GetEmployeeStatus {UserId = user.Id});
        }
    }
}