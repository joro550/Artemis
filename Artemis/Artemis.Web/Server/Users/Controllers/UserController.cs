using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Artemis.Web.Server.Users.Models;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.Web.Server.Users.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user/")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("permission/subscribe")]
        public async Task<bool> CanSubscribe()
        {
            var user = await _userManager.GetUserAsync(User);
            return !string.IsNullOrWhiteSpace(user.PhoneNumber);
        }
    }
}