using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Artemis.Web.Client.Events;
using Artemis.Web.Shared.Events;
using Artemis.Web.Shared.Organizations;

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
        public bool IsPublished { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [Required] 
        public virtual EventType EventType { get; set; } = EventType.Persistent;

        public EventAddressEntity Address { get; set; }
        public OrganizationEntity Organization { get; set; }

        public List<EventUpdateEntity> Updates { get; set; }
    }
}