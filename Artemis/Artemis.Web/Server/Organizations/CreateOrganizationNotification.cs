using MediatR;

namespace Artemis.Web.Server.Organizations
{
    public class CreateOrganizationNotification : INotification
    {
        public string Name { get; set; }
    }
}