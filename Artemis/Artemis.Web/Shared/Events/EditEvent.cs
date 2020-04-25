namespace Artemis.Web.Shared.Events
{
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