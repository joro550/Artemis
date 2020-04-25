using Artemis.Web.Shared.MessageTemplates;
using MediatR;

namespace Artemis.Web.Server.MessageTemplates
{
    public class GetMessageTemplate : IRequest<MessageTemplate>
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
    }
}