using Artemis.Web.Shared.EventUpdates;
using MediatR;

namespace Artemis.Web.Server.EventUpdates
{
    public class GetEventUpdate : IRequest<EventUpdate>
    {
        public int EventId { get; set; }
        public int UpdateId { get; set; }
        public int OrganizationId { get; set; }

        public int Count { get; set; }
        public int Offset { get; set; }
    }
}