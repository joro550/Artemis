using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.EventUpdates
{
    public class EditEventUpdate : EventUpdateModelBase
    {
        [Required]
        public int Id { get; set; }

        public void Load(EventUpdate update)
        {
            Id = update.Id;
            Title = update.Title;
            Message = update.Message;
            EventId = update.EventId;
        }
    }
}