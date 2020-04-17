using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.MessageTemplates
{
    public class MessageTemplateModelBase
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public MessageEvent MessageEvent { get; set; }
    }
}