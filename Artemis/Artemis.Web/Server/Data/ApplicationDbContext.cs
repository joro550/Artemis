using Artemis.Web.Server.Models;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Data.Models;
using IdentityServer4.EntityFramework.Options;
using EventType = Artemis.Web.Shared.Events.EventType;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;

namespace Artemis.Web.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasDiscriminator(b => b.EventType)
                .HasValue<Event>(EventType.Persistent)
                .HasValue<TimedEvent>(EventType.Timed);

            modelBuilder.Entity<Event>()
                .HasOne<Organization>()
                .WithMany(organization => organization.Events)
                .HasForeignKey(ev => ev.OrganizationId);
        }
    }
}
