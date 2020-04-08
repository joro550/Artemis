using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Web.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionController : ControllerBase

    {
        private readonly IMediator _mediator;

        public SubscriptionController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("location")]
        public Task CreateSubscriptionBasedOnLocation()
        {
            var user = this.User.Identity;
            return Task.CompletedTask;
        }


        [HttpPost("organization")]
        public Task CreateSubscriptionToOrganization()
        {
            return Task.CompletedTask;
        }

        [HttpPost("event")]
        public Task CreateSubscriptionToEvent()
        {
            return Task.CompletedTask;
        }

    }
}