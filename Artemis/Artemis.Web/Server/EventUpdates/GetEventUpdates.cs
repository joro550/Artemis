using MediatR;
using System.Collections.Generic;
using Artemis.Web.Shared.EventUpdates;

namespace Artemis.Web.Server.EventUpdates
{
    public class GetEventUpdates : IRequest<List<EventUpdate>>
    {
        public int EventId { get; set; }
        public int OrganizationId { get; set; }

        public int Count { get; set; }
        public int Offset { get; set; }
    }
}