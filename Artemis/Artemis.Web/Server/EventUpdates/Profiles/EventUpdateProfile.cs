using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.EventUpdates;
using AutoMapper;

namespace Artemis.Web.Server.EventUpdates.Profiles
{
    public class EventUpdateProfile : Profile
    {
        public EventUpdateProfile()
        {
            CreateMap<EventUpdateEntity, EventUpdate>();
            CreateMap<CreateEventUpdate, EventUpdateEntity>();
        }
    }
}