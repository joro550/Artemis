using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.EventUpdates
{
    public class EventUpdateModelBase
    {
        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

    }
}