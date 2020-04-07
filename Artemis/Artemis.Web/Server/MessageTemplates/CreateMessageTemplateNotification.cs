using Artemis.Web.Shared.MessageTemplates;
using MediatR;

namespace Artemis.Web.Server.MessageTemplates
{
    public class CreateMessageTemplateNotification : INotification
    {
        public CreateMessageTemplate Model { get; set; }
    }
}