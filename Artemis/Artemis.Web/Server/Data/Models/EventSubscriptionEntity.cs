using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Artemis.Web.Shared.Subscriptions;

namespace Artemis.Web.Server.Data.Models
{
    public class EventSubscriptionEntity : UserSubscriptionEntity
    {
        [Required]
        [Column("SubscribedTo")]
        public int EventId { get; set; }
        public override SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Event;
    }
}