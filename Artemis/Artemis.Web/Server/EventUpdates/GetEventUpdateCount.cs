using MediatR;

namespace Artemis.Web.Server.EventUpdates
{
    public class GetEventUpdateCount : IRequest<int>
    {
        public int? EventId { get; set; }
    }
}