using System.Collections.Generic;
using Artemis.Web.Shared.MessageTemplates;
using MediatR;

namespace Artemis.Web.Server.MessageTemplates
{
    public class GetMessageTemplates : IRequest<List<MessageTemplate>>
    {
        public int OrganizationId { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
    }
}