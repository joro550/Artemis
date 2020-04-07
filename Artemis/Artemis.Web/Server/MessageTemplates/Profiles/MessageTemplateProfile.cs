using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.MessageTemplates;
using AutoMapper;

namespace Artemis.Web.Server.MessageTemplates.Profiles
{
    public class MessageTemplateProfile : Profile
    {
        public MessageTemplateProfile()
        {
            CreateMap<MessageTemplateEntity, MessageTemplate>();
            CreateMap<CreateMessageTemplate, MessageTemplateEntity>();
        }
    }
}