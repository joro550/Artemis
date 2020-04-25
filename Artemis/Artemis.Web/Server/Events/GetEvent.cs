using Artemis.Web.Shared.Events;
using MediatR;

namespace Artemis.Web.Server.Events
{
    public class GetEvent : IRequest<Event>
    {
        public string? UserId { get; set; }
        public int OrganizationId { get; set; }
        public int Id { get; set; }
    }
}