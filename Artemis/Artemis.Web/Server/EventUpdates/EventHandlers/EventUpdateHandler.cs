using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Server.EventUpdates.Events;
using Artemis.Web.Shared.EventUpdates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Web.Server.EventUpdates.EventHandlers
{
    public class EventUpdateHandler
        :   IRequestHandler<GetEventUpdate, EventUpdate>,
            IRequestHandler<GetEventUpdateCount, int>,
            INotificationHandler<EditEventUpdateNotification>,
            IRequestHandler<GetEventUpdates, List<EventUpdate>>,
            INotificationHandler<CreateEventUpdateNotification>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public EventUpdateHandler(ApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _context = context;
            _mediator = mediator;
        }

        public async Task<List<EventUpdate>> Handle(GetEventUpdates request, CancellationToken cancellationToken)
        {
            var updates = await _context.Set<EventUpdateEntity>()
                .Where(entity => entity.EventId == request.EventId)
                .Skip(request.Offset * request.Count)
                .Take(request.Count)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<EventUpdate>>(updates ?? new List<EventUpdateEntity>());
        }

        public async Task<int> Handle(GetEventUpdateCount request, CancellationToken cancellationToken)
        {
            var updates = _context.Set<EventUpdateEntity>()
                .AsQueryable();
            
            if (request.EventId.HasValue)
                updates = updates.Where(entity => entity.EventId == request.EventId.Value);
            return await updates.CountAsync(cancellationToken);
        }

        public async Task<EventUpdate> Handle(GetEventUpdate request, CancellationToken cancellationToken)
        {
            var update = await _context.Set<EventUpdateEntity>()
                .FirstOrDefaultAsync(entity => entity.Id == request.UpdateId, cancellationToken);
            return _mapper.Map<EventUpdate>(update);
        }

        public async Task Handle(CreateEventUpdateNotification notification, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<EventUpdateEntity>(notification.Model);
            var result = await _context.Set<EventUpdateEntity>().AddAsync(request, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new EventUpdateCreated {Id = result.Entity.Id}, cancellationToken);
        }

        public async Task Handle(EditEventUpdateNotification notification, CancellationToken cancellationToken)
        {
            var updateEntity = _mapper.Map<EventUpdateEntity>(notification.Model);
            _context.Update(updateEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}