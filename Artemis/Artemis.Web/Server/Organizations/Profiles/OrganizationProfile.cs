using AutoMapper;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.Organizations;

namespace Artemis.Web.Server.Organizations.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<OrganizationEntity, Organization>();
            CreateMap<CreateOrganization, OrganizationEntity>();
            CreateMap<EditOrganization, OrganizationEntity>();
        }
    }
}