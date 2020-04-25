using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.MessageTemplates;

namespace Artemis.Web.Server.MessageTemplates.EventHandlers
{
    public class MessageTemplateHandler
        :   IRequestHandler<GetMessageTemplates, List<MessageTemplate>>,    
            IRequestHandler<GetMessageTemplateCount, int>,
            IRequestHandler<GetMessageTemplate, MessageTemplate>,
            INotificationHandler<CreateMessageTemplateNotification>,
            INotificationHandler<EditMessageTemplateNotification>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public MessageTemplateHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<MessageTemplate>> Handle(GetMessageTemplates request, CancellationToken cancellationToken)
        {
            var templates = await _context.Set<MessageTemplateEntity>()
                .Where(template => template.OrganizationId == request.OrganizationId)
                .Skip(request.Offset * request.Count)
                .Take(request.Count)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<MessageTemplate>>(templates ?? new List<MessageTemplateEntity>());
        }

        public async Task<int> Handle(GetMessageTemplateCount request, CancellationToken cancellationToken)
        {
            return await _context.Set<MessageTemplateEntity>()
                .Where(entity => entity.OrganizationId == request.OrganizationId)
                .CountAsync(cancellationToken);
        }

        public async Task<MessageTemplate> Handle(GetMessageTemplate request, CancellationToken cancellationToken)
        {
            var template = await _context.Set<MessageTemplateEntity>()
                .SingleOrDefaultAsync(entity => entity.Id == request.Id, cancellationToken);
            return _mapper.Map<MessageTemplate>(template ?? new MessageTemplateEntity());
        }

        public async Task Handle(CreateMessageTemplateNotification notification, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<MessageTemplateEntity>(notification.Model);
            var response = await _context.Set<MessageTemplateEntity>()
                .AddAsync(request, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(EditMessageTemplateNotification notification, CancellationToken cancellationToken)
        {
            var templateEntity = _mapper.Map<MessageTemplateEntity>(notification.Model);
            
            _context.Set<MessageTemplateEntity>().Update(templateEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}