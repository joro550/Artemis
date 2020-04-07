using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Artemis.Web.Shared.MessageTemplates;

namespace Artemis.Web.Server.Data.Models
{
    public class MessageTemplateEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public MessageEvent MessageEvent { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string Text { get; set; }
    }
}