﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.Events
{
    public class CreateEvent
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public int OrganizationId { get; set; }
        
        [Required]
        public bool IsTimedEvent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}