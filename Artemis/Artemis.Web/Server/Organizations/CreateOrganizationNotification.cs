using Artemis.Web.Shared.Organizations;
using MediatR;

namespace Artemis.Web.Server.Organizations
{
    public class CreateOrganizationNotification : INotification
    {
        public CreateOrganization Organization { get; set; }
    }
}