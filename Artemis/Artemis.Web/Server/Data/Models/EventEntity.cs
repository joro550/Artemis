using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Artemis.Web.Shared.Events;

namespace Artemis.Web.Server.Data.Models
{
    [Table("Event")]
    public class EventEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required] 
        public virtual EventType EventType { get; set; } = EventType.Persistent;

        [Required]
        public int OrganizationId { get; set; }

        public EventAddressEntity Address { get; set; }
    }
}