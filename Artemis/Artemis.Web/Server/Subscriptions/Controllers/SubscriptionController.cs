﻿using System.Threading.Tasks;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Shared.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Web.Server.Subscriptions.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost("location")]
        public async Task<LocationSubscriptionResult> CreateSubscriptionBasedOnLocation(LocationSubscriptionRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            await _mediator.Publish(new CreateLocationSubscription
            {
                UserId = user.Id, 
                OrganizationId = request.OrganizationId
            });

            return new LocationSubscriptionResult {Success = true};
        }

        [HttpPost("organization")]
        public async Task<OrganizationSubscriptionResult> CreateSubscriptionToOrganization(OrganizationSubscriptionRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            await _mediator.Publish(new CreateOrganizationSubscription
            {
                UserId = user.Id,
                OrganizationId = request.OrganizationId
            });
            return new OrganizationSubscriptionResult {Success = true};
        }

        [HttpPost("event")]
        public async Task<EventSubscriptionResult> CreateSubscriptionToEvent(EventSubscriptionRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            await _mediator.Publish(new CreateEventSubscription
            {
                UserId = user.Id,
                EventId = request.EventId
            });
            return new EventSubscriptionResult{Success = true};
        }
    }
}