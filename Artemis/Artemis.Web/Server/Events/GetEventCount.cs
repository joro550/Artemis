using MediatR;

namespace Artemis.Web.Server.Events
{
    public class GetEventCount : IRequest<int>
    {
        public int? OrganizationId { get; set; }
        public string? UserId { get; set; }
    }
}