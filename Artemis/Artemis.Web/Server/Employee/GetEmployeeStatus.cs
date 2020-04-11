using MediatR;
using System.Collections.Generic;
using Artemis.Web.Shared.Employee;

namespace Artemis.Web.Server.Employee
{
    public class GetEmployeeStatus : IRequest<List<EmployeeStatusResponse>>
    {
        public string UserId { get; set; }
    }
}