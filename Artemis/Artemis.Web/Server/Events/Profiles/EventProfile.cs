using AutoMapper;
using Artemis.Web.Shared.Events;
using Artemis.Web.Server.Data.Models;

namespace Artemis.Web.Server.Events.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventEntity, Event>()
                .ReverseMap();

            CreateMap<EventEntity, CreateEvent>()
                .ReverseMap();

            CreateMap<TimedEventEntity, CreateEvent>()
                .ReverseMap();
        }
    }
}