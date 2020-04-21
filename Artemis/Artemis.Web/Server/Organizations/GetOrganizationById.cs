using Artemis.Web.Shared.Organizations;
using MediatR;

namespace Artemis.Web.Server.Organizations
{
    public class GetOrganizationById : IRequest<Organization>
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
    }
}