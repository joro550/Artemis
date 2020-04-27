using System.Security.Claims;
using System.Threading.Tasks;
using Artemis.Web.Server.Users;
using Artemis.Web.Server.Users.Models;

namespace Artemis.Web.Server.Tests.Organizations
{
    public class FakeUserAdapter : IUserAdapter 
    { 
        public Task<UserModel> GetUserAsync(ClaimsPrincipal principal) 
            => Task.FromResult((UserModel) new TestUser());
    }
}