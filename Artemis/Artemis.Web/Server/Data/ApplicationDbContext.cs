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
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<OrganizationEntity> Organizations { get; set; }
        public DbSet<EventAddressEntity> EventAddresses { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventEntity>()
                .HasDiscriminator(b => b.EventType)
                .HasValue<EventEntity>(EventType.Persistent)
                .HasValue<TimedEventEntity>(EventType.Timed);

            modelBuilder.Entity<EventAddressEntity>()
                .HasOne<EventEntity>()
                .WithOne(entity => entity.Address)
                .HasForeignKey<EventAddressEntity>(entity => entity.EventId);

            modelBuilder.Entity<EventEntity>()
                .HasOne<OrganizationEntity>()
                .WithMany(organization => organization.Events)
                .HasForeignKey(ev => ev.OrganizationId);
        }
    }
}
