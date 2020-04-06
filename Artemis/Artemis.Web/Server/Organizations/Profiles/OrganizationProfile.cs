using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.Organizations;
using AutoMapper;

namespace Artemis.Web.Server.Organizations.Profiles
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<OrganizationEntity, Organization>();
        }
    }
}