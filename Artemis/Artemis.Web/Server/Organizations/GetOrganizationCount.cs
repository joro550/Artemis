using MediatR;

namespace Artemis.Web.Server.Organizations
{
    public class GetOrganizationCount : IRequest<int>
    {
        public string? UserId { get; set; }
    }
}