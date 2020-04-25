using MediatR;

namespace Artemis.Web.Server.MessageTemplates
{
    public class GetMessageTemplateCount : IRequest<int>
    {
        public int OrganizationId { get; set; }
    }
}