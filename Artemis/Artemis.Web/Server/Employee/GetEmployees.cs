using MediatR;
using System.Collections.Generic;
using Artemis.Web.Shared.Employee;

namespace Artemis.Web.Server.Employee
{
    public class GetEmployees : IRequest<List<EmployeeStatusResponse>>
    {
        public int OrganizationId { get; set; }
    }
}