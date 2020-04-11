using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using System.Collections.Generic;
using Artemis.Web.Shared.Employee;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Server.Organizations.Notifications;

namespace Artemis.Web.Server.Employee.EventHandlers
{
    public class OrganizationEmployeeHandler
        :   INotificationHandler<OrganizationCreated>,
            IRequestHandler<GetEmployeeStatus, List<EmployeeStatusResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public OrganizationEmployeeHandler(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(OrganizationCreated notification, CancellationToken cancellationToken)
        {
            await _context.Employees.AddAsync(cancellationToken: cancellationToken,
                entity: new EmployeeEntity {UserId = notification.UserId, OrganizationId = notification.Id});
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<EmployeeStatusResponse>> Handle(GetEmployeeStatus request, CancellationToken cancellationToken)
        {
            var employeeStatus = await _context.Set<EmployeeEntity>()
                .Where(entity => entity.UserId == request.UserId)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<EmployeeStatusResponse>>(employeeStatus);
        }
    }
}