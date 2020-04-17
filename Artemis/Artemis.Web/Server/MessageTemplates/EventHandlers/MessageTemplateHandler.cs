using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Web.Client.Events;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.MessageTemplates;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Web.Server.MessageTemplates.EventHandlers
{
    public class MessageTemplateHandler
        :   IRequestHandler<GetMessageTemplates, List<MessageTemplate>>,    
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
                .Skip(request.Offset)
                .Take(request.Count)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<MessageTemplate>>(templates ?? new List<MessageTemplateEntity>());
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

        public Task Handle(EditMessageTemplateNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}