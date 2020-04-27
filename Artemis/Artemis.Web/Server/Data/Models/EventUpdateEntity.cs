﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artemis.Web.Server.Data.Models
{
    [Table("EventUpdate")]
    public class EventUpdateEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int EventId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }
        
        public EventEntity Event { get; set; }
        
        public string CallToAction { get; set; }
        public string CallToActionText { get; set; }

        public DateTime UpdateCreatedTime { get; set; }
    }
}
