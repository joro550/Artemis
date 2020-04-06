using System;

namespace Artemis.Web.Shared.Events
{
    public class TimedEvent : Event
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}