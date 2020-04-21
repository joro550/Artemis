using MediatR;
using Artemis.Web.Shared.EventUpdates;

namespace Artemis.Web.Server.EventUpdates
{
    public class GetEventUpdate : IRequest<EventUpdate>
    {
        public int EventId { get; set; }
        public int UpdateId { get; set; }
        public int OrganizationId { get; set; }
    }
}