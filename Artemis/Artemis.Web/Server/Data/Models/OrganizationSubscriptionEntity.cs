using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Artemis.Web.Shared.Subscriptions;

namespace Artemis.Web.Server.Data.Models
{
    public class OrganizationSubscriptionEntity : UserSubscriptionEntity
    {
        [Required]
        [Column("SubscribedTo")]
        public int OrganizationId { get; set; }

        public override SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Organization;
    }
}