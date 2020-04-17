using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.EventUpdates
{
    public class EditEventUpdate : EventUpdateModelBase
    {
        [Required]
        public int Id { get; set; }
    }
}