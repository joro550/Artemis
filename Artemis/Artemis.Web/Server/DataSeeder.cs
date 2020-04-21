using Bogus;
using System;
using System.Linq;
using System.Threading.Tasks;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Config;
using Artemis.Web.Shared.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Artemis.Web.Server.Data.Models;
using Artemis.Web.Server.Users.Models;
using Artemis.Web.Shared.MessageTemplates;

namespace Artemis.Web.Server
{
    public class DataSeeder
    {
        private readonly UserConfig _userConfig;
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataSeeder(ApplicationDbContext context, IHostEnvironment environment, 
            UserManager<ApplicationUser> userManager, IOptions<UserConfig> userConfig)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _userConfig = userConfig.Value;
        }

        public async Task Plant()
        {
            if (!_environment.IsDevelopment())
                return;

            Randomizer.Seed = new Random(8675309);

            await _userManager.CreateAsync(password: "Ii!62s9cVB&%^hF8",
                user: new ApplicationUser {Email = "user@email.com", PhoneNumber = _userConfig.PhoneNumber, UserName = "user@email.com", EmailConfirmed = true,PhoneNumberConfirmed = true});

            var user = await _userManager.FindByEmailAsync("user@email.com");
            
            var organizations = new Faker<OrganizationEntity>()
                    .RuleFor(entity => entity.IsPublished, faker => true)
                    .RuleFor(entity => entity.Name, faker => faker.Company.CompanyName())
                    .RuleFor(entity => entity.Description, faker => faker.Company.Bs());

            const int maxRecords = 51;

            var count = 0;
            foreach (var entity in organizations.GenerateForever())
            {
                await _context.Organizations.AddAsync(entity);
                count++;
                if (count < maxRecords) continue;

                await _context.SaveChangesAsync();
                break;
            }

            foreach (var organizationEntity in _context.Organizations.ToList())
            {
                await _context.Employees.AddAsync(new EmployeeEntity
                    {OrganizationId = organizationEntity.Id, UserId = user.Id});

                await CreateEvents(organizationEntity, maxRecords);
                await CreateTemplates(organizationEntity, maxRecords);
            }

            await _context.SaveChangesAsync();
        }

        private async Task CreateEvents(OrganizationEntity org, int maxRecords)
        {
            var events = new Faker<EventEntity>()
                .RuleFor(entity => entity.IsPublished, faker => true)
                .RuleFor(entity => entity.Name, faker => faker.Lorem.Lines(1))
                .RuleFor(entity => entity.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(entity => entity.EventType, faker => faker.Random.Enum<EventType>());

            var count = 0;

            foreach (var eventEntity in events.GenerateForever())
            {
                eventEntity.OrganizationId = org.Id;
                await _context.Events.AddAsync(eventEntity);
                
                count++;

                if (count < maxRecords) continue;

                await _context.SaveChangesAsync();
                break;
            }

            foreach (var eventEntity in _context.Events.Where(evt => evt.OrganizationId == org.Id))
            {
                await CreateEventUpdates(eventEntity, maxRecords);
            }
        }

        private async Task CreateEventUpdates(EventEntity eventEntity, int maxRecords)
        {
            var updates = new Faker<EventUpdateEntity>()
                .RuleFor(entity => entity.Title, faker => string.Join(" ", faker.Lorem.Words()))
                .RuleFor(entity => entity.Message, faker => faker.Lorem.Paragraphs(3));

            var count = 0;
            foreach (var update in updates.GenerateForever())
            {
                update.EventId = eventEntity.Id;

                await _context.EventUpdate.AddAsync(update);

                count++;
                if (count < maxRecords) continue;

                await _context.SaveChangesAsync();
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

            var count = 0;
            foreach (var @event in template.GenerateForever())
            {
                @event.OrganizationId = org.Id;
                await _context.MessageTemplates.AddAsync(@event);

                count++;
                if (count < maxRecords) continue;
                
                await _context.SaveChangesAsync();
                break;
            }
        }
    }
}