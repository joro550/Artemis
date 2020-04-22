using Bogus;
using System;
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
        private Faker<EventEntity> _events;
        private Faker<EventUpdateEntity> _updates;
        private Faker<MessageTemplateEntity> _template;
        private Faker<OrganizationEntity> _organizations;

        public DataSeeder(ApplicationDbContext context, IHostEnvironment environment, 
            UserManager<ApplicationUser> userManager, IOptions<UserConfig> userConfig)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _userConfig = userConfig.Value;

            _organizations = new Faker<OrganizationEntity>()
                .RuleFor(entity => entity.Id, faker => faker.IndexFaker++)
                .RuleFor(entity => entity.IsPublished, faker => true)
                .RuleFor(entity => entity.Name, faker => faker.Company.CompanyName())
                .RuleFor(entity => entity.Description, faker => faker.Company.Bs());

            _events = new Faker<EventEntity>()
                .RuleFor(entity => entity.Id, faker => faker.IndexFaker++)
                .RuleFor(entity => entity.IsPublished, faker => true)
                .RuleFor(entity => entity.Name, faker => faker.Lorem.Lines(1))
                .RuleFor(entity => entity.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(entity => entity.EventType, faker => faker.Random.Enum<EventType>());

            _updates = new Faker<EventUpdateEntity>()
                .RuleFor(entity => entity.Id, faker => faker.IndexFaker++)
                .RuleFor(entity => entity.Title, faker => string.Join(" ", faker.Lorem.Words()))
                .RuleFor(entity => entity.Message, faker => faker.Lorem.Paragraphs(3));

            _template = new Faker<MessageTemplateEntity>()
                .RuleFor(entity => entity.Id, faker => faker.IndexFaker++)
                .RuleFor(entity => entity.IsActive, faker => faker.Random.Bool())
                .RuleFor(entity => entity.Name, faker => faker.Lorem.Word())
                .RuleFor(entity => entity.Text, faker => faker.Lorem.Paragraph())
                .RuleFor(entity => entity.MessageEvent, faker => faker.Random.Enum<MessageEvent>());
        }

        public async Task Plant()
        {
            if (!_environment.IsDevelopment())
                return;

            Randomizer.Seed = new Random(8675309);

            await _userManager.CreateAsync(password: "Ii!62s9cVB&%^hF8",
                user: new ApplicationUser {Email = "user@email.com", PhoneNumber = _userConfig.PhoneNumber, UserName = "user@email.com", EmailConfirmed = true,PhoneNumberConfirmed = true});

            const int maxRecords = 50;
            var user = await _userManager.FindByEmailAsync("user@email.com");

            var count = 0;
            foreach (var entity in _organizations.GenerateForever())
            {
                await _context.Organizations.AddAsync(entity);
                await _context.Employees.AddAsync(new EmployeeEntity
                    { OrganizationId = entity.Id, UserId = user.Id });

                await CreateEvents(entity, maxRecords);
                await CreateTemplates(entity, maxRecords);

                count++;
                if (count >= maxRecords) break;
            }

            await _context.SaveChangesAsync();
        }

        private async Task CreateEvents(OrganizationEntity org, int maxRecords)
        {
            var count = 0;
            foreach (var eventEntity in _events.GenerateForever())
            {
                eventEntity.OrganizationId = org.Id;
                await _context.Events.AddAsync(eventEntity);
                await CreateEventUpdates(eventEntity, maxRecords);

                count++;
                if (count >= maxRecords) break;
            }
        }

        private async Task CreateEventUpdates(EventEntity eventEntity, int maxRecords)
        {
            var count = 0;
            foreach (var update in _updates.GenerateForever())
            {
                update.EventId = eventEntity.Id;
                await _context.EventUpdate.AddAsync(update);

                count++;
                if (count >= maxRecords) break;
            }
        }

        private async Task CreateTemplates(OrganizationEntity org, int maxRecords)
        {
            var count = 0;
            foreach (var @event in _template.GenerateForever())
            {
                @event.OrganizationId = org.Id;
                await _context.MessageTemplates.AddAsync(@event);

                count++;
                if (count >= maxRecords) break;
            }
        }
    }
}