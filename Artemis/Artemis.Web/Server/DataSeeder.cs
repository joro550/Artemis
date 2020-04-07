using Bogus;
using System;
using System.Linq;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Shared.Events;
using Microsoft.Extensions.Hosting;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Shared.MessageTemplates;

namespace Artemis.Web.Server
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _environment;

        public DataSeeder(ApplicationDbContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task Plant()
        {
            if (!_environment.IsDevelopment())
                return;

            Randomizer.Seed = new Random(8675309);

            var organizations = new Faker<OrganizationEntity>()
                    .RuleFor(entity => entity.Name, faker => faker.Company.CompanyName())
                    .RuleFor(entity => entity.Description, faker => faker.Company.Bs());




            var maxRecords = 12;

            foreach (var org in organizations.GenerateForever())
            {
                await _context.Organizations.AddAsync(org);
                await _context.SaveChangesAsync();

                await CreateEvents(org, maxRecords);
                await CreateTemplates(org, maxRecords);

                if (_context.Organizations.Count() >= maxRecords)
                    break;
            }

            await _context.SaveChangesAsync();
        }

        private async Task CreateEvents(OrganizationEntity org, int maxRecords)
        {
            var events = new Faker<EventEntity>()
                .RuleFor(entity => entity.Name, faker => faker.Lorem.Lines(1))
                .RuleFor(entity => entity.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(entity => entity.EventType, faker => faker.Random.Enum<EventType>());

            foreach (var @event in events.GenerateForever())
            {
                @event.OrganizationId = org.Id;
                await _context.Events.AddAsync(@event);
                await _context.SaveChangesAsync();

                var count = _context.Events.Count(entity => entity.OrganizationId == org.Id);

                if (count >= maxRecords)
                    break;
            }
        }



        private async Task CreateTemplates(OrganizationEntity org, int maxRecords)
        {
            var template = new Faker<MessageTemplateEntity>()
                .RuleFor(entity => entity.IsActive, faker => faker.Random.Bool())
                .RuleFor(entity => entity.Name, faker => faker.Lorem.Word())
                .RuleFor(entity => entity.Text, faker => faker.Lorem.Paragraph())
                .RuleFor(entity => entity.MessageEvent, faker => faker.Random.Enum<MessageEvent>());

            foreach (var @event in template.GenerateForever())
            {
                @event.OrganizationId = org.Id;
                await _context.MessageTemplates.AddAsync(@event);
                await _context.SaveChangesAsync();

                var count = _context.MessageTemplates.Count(entity => entity.OrganizationId == org.Id);

                if (count >= maxRecords)
                    break;
            }
        }
    }
}