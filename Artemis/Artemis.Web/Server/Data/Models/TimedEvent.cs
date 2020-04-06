using System;

namespace Artemis.Web.Server.Data.Models
{
    public class TimedEvent : Event
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}