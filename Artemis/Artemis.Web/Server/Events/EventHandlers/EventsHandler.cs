using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Shared.Events;
using System.Collections.Generic;
using Artemis.Web.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Web.Server.Events.EventHandlers
{
    public class EventsHandler
        : IRequestHandler<GetEvent, Event>, 
          IRequestHandler<GetEvents, List<Event>>,
          INotificationHandler<CreateEventNotification>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public EventsHandler(IMapper mapper, ApplicationDbContext context, IMediator mediator)
        {
            _mapper = mapper;
            _context = context;
            _mediator = mediator;
        }

        public async Task<Event> Handle(GetEvent request, CancellationToken cancellationToken)
        {
            var @event = await _context.Set<EventEntity>()
                .SingleOrDefaultAsync(org => org.Id == request.Id, cancellationToken);
            return _mapper.Map<Event>(@event ?? new EventEntity());
        }

        public async Task<List<Event>> Handle(GetEvents request, CancellationToken cancellationToken)
        {
            var dbSet = _context.Set<EventEntity>();

            var query = string.IsNullOrWhiteSpace(request.UserId)
                ? dbSet.Where(entity => entity.IsPublished)
                : dbSet.Include(entity => entity.Organization).ThenInclude(organization => organization.Employees)
                    .Where(entity => entity.IsPublished || entity.Organization.Employees.Any(employeeEntity => employeeEntity.UserId == request.UserId));

            var events = await query.Where(entity => entity.OrganizationId == request.OrganizationId)
                .Skip(request.Offset)
                .Take(request.Count)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<Event>>(events);
        }

        public async Task Handle(CreateEventNotification notification, CancellationToken cancellationToken)
        {
            var eventEntity = notification.Event.IsTimedEvent 
                ? _mapper.Map<TimedEventEntity>(notification.Event) 
                : _mapper.Map<EventEntity>(notification.Event);

            await _context.AddAsync(eventEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new EventCreatedNotification {Id = eventEntity.Id}, cancellationToken);
        }
    }
}
