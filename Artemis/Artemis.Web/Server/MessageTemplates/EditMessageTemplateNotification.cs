using Artemis.Web.Shared.MessageTemplates;
using MediatR;

namespace Artemis.Web.Server.MessageTemplates
{
    public class EditMessageTemplateNotification : INotification
    {
        public EditMessageTemplate Model { get; set; }
    }
}