using Artemis.Web.Shared.Organizations;
using MediatR;

namespace Artemis.Web.Server.Organizations
{
    public class EditOrganizationNotification : INotification
    {
        public string UserId { get; set; }
        public EditOrganization Organization { get; set; }
    }
}