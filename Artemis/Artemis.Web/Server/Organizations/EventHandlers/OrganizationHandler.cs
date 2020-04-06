using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Server.Organizations.Notifications;
using Artemis.Web.Shared.Organizations;

namespace Artemis.Web.Server.Organizations.EventHandlers
{
    public class OrganizationHandler
        : IRequestHandler<GetOrganizationById, Organization>,
          IRequestHandler<GetOrganizations, List<Organization>>,
          INotificationHandler<CreateOrganizationNotification>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public OrganizationHandler(IMapper mapper, IMediator mediator, ApplicationDbContext context)
        {
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }

        public async Task<Organization> Handle(GetOrganizationById request, CancellationToken cancellationToken)
        {
            var organization = await _context.Set<OrganizationEntity>()
                .SingleOrDefaultAsync(org => org.Id == request.Id, cancellationToken);
            return _mapper.Map<Organization>(organization ?? new OrganizationEntity());
        }

        public async Task<List<Organization>> Handle(GetOrganizations request, CancellationToken cancellationToken)
        {
            var organization = await _context.Set<OrganizationEntity>()
                .Skip(request.Offset)
                .Take(request.Count)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<Organization>>(organization);
        }

        public async Task Handle(CreateOrganizationNotification notification, CancellationToken cancellationToken)
        {
            var result = await _context.Set<OrganizationEntity>()
                .AddAsync(new OrganizationEntity {Name = notification.Name}, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // await _mediator.Publish(new OrganizationCreated { Id = result.Entity.Id, Name = result.Entity.Name}, cancellationToken);
        }
    }
}