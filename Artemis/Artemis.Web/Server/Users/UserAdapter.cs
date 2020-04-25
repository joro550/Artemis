using MediatR;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Artemis.Web.Server.Users.Models;

namespace Artemis.Web.Server.Users
{
    public class UserAdapter
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAdapter(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public virtual async Task<UserModel> GetUserAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return new UserModel(user, _mediator);
        }
    }
}