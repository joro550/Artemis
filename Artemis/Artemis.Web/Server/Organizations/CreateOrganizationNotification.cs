using Artemis.Web.Shared.Organizations;
using MediatR;

namespace Artemis.Web.Server.Organizations
{
    public class CreateOrganizationNotification : INotification
    {
        public string? UserId { get; set; }
        public CreateOrganization Organization { get; set; }
    }
}