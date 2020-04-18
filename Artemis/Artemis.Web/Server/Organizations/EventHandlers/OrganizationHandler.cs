using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.Organizations;
using Artemis.Web.Server.Organizations.Notifications;

namespace Artemis.Web.Server.Organizations.EventHandlers
{
    public class OrganizationHandler
        : IRequestHandler<GetOrganizationById, Organization>,
          IRequestHandler<GetOrganizations, List<Organization>>,
          INotificationHandler<CreateOrganizationNotification>,
          INotificationHandler<EditOrganizationNotification>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public OrganizationHandler(IMapper mapper, ApplicationDbContext context, IMediator mediator)
        {
            _mapper = mapper;
            _context = context;
            _mediator = mediator;
        }

        public async Task<Organization> Handle(GetOrganizationById request, CancellationToken cancellationToken)
        {
            var organization = await _context.Set<OrganizationEntity>()
                .AsNoTracking()
                .SingleOrDefaultAsync(org => org.Id == request.Id, cancellationToken);
            return _mapper.Map<Organization>(organization ?? new OrganizationEntity());
        }

        public async Task<List<Organization>> Handle(GetOrganizations request, CancellationToken cancellationToken)
        {
            var dbSet = _context.Set<OrganizationEntity>();

            var query = string.IsNullOrWhiteSpace(request.UserId) 
                ? dbSet.Where(entity => entity.IsPublished) 
                : dbSet.Include(entity => entity.Employees)
                    .Where(entity => entity.IsPublished || entity.Employees.Any(employee => employee.UserId == request.UserId));

             var organization = await query.Skip(request.Offset)
                .Take(request.Count)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<Organization>>(organization);
        }

        public async Task Handle(CreateOrganizationNotification notification, CancellationToken cancellationToken)
        {
            var organization = _mapper.Map<OrganizationEntity>(notification.Organization);
            var result = await _context.Set<OrganizationEntity>()
                .AddAsync(organization, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new OrganizationCreated {Id = result.Entity.Id, UserId = notification.UserId},
                cancellationToken);
        }

        public async Task Handle(EditOrganizationNotification notification, CancellationToken cancellationToken)
        {
            var organizationEntity = _mapper.Map<OrganizationEntity>(notification.Organization);

            var orgFromDb = await _context.Set<OrganizationEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Id == notification.Organization.Id, cancellationToken);

            organizationEntity.IsPublished = orgFromDb.IsPublished;
            _context.Set<OrganizationEntity>().Update(organizationEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}