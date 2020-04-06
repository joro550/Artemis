using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.Organizations;

namespace Artemis.Web.Server.Organizations.EventHandlers
{
    public class GetOrganizationHandler
        : IRequestHandler<GetOrganizationById, Organization>,
          IRequestHandler<GetOrganizations, List<Organization>>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetOrganizationHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Organization> Handle(GetOrganizationById request, CancellationToken cancellationToken)
        {
            var organization = await _context.Set<OrganizationEntity>()
                .SingleOrDefaultAsync(org => org.Id == request.Id, cancellationToken);
            return _mapper.Map<Organization>(organization ?? new OrganizationEntity());
        }

        public Task<List<Organization>> Handle(GetOrganizations request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<Organization>());
        }
    }
}