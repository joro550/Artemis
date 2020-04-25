using System.Collections.Generic;
using Artemis.Web.Shared.Events;
using MediatR;

namespace Artemis.Web.Server.Events
{
    public class GetEvents : IRequest<List<Event>>
    {
        public string? UserId { get; set; }
        public int OrganizationId { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
    }
}