using System.Collections.Generic;
using System.Threading.Tasks;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Shared.Employee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Web.Server.Employee.Controllers
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