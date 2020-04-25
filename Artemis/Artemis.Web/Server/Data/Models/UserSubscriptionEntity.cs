using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Artemis.Web.Shared.Subscriptions;

namespace Artemis.Web.Server.Data.Models
{
    [Table("UserSubscription")]
    public abstract class UserSubscriptionEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public virtual SubscriptionType SubscriptionType { get; set; }
    }
}