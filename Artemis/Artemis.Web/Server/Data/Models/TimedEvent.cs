using System;

namespace Artemis.Web.Server.Data.Models
{
    public class TimedEvent : EventEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}