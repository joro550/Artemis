using System.Collections.Generic;
using Artemis.Web.Client.Events;
using Artemis.Web.Shared.Events;
using MediatR;

namespace Artemis.Web.Server.Events
{
    public class GetEvent : IRequest<Event>
    {
        public int OrganizationId { get; set; }
        public int Id { get; set; }
    }

    public class GetEvents : IRequest<List<Event>>
    {
        public int OrganizationId { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
    }

    public class CreateEventNotification : INotification
    {
        public CreateEvent Event { get; set; } 
    }
}