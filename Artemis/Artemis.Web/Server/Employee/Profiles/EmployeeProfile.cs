using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.Employee;
using AutoMapper;

namespace Artemis.Web.Server.Employee.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeEntity, EmployeeStatusResponse>();
        }
    }
}