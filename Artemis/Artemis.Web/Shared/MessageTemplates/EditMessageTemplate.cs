using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.MessageTemplates
{
    public class EditMessageTemplate : MessageTemplateModelBase
    {
        [Required]
        public int Id { get; set; }
    }
}