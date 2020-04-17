using System;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.Events
{

    public class BaseEventModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public bool IsTimedEvent { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

    }

    public class CreateEvent : BaseEventModel
    {
    }


    public class EditEvent : BaseEventModel
    {
        public int Id { get; set; }

        public void Load(Event evt)
        {
            Id = evt.Id;
            Name = evt.Name;
            Description = evt.Description;
            OrganizationId = evt.OrganizationId;

            if (!(evt is TimedEvent timedEvent)) 
                return;

            IsTimedEvent = true;
            EndDate = timedEvent.EndDate;
            StartDate = timedEvent.StartDate;
        }
    }
}