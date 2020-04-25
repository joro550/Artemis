using System;
using Artemis.Web.Shared.Events;

namespace Artemis.Web.Server.Data.Models
{
    public class TimedEventEntity : EventEntity
    {
        public override EventType EventType { get; set; } = EventType.Timed;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}