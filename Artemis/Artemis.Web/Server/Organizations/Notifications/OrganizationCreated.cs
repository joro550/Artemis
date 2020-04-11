using MediatR;

namespace Artemis.Web.Server.Organizations.Notifications
{
    public class OrganizationCreated : INotification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}