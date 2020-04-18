using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.MessageTemplates
{
    public class EditMessageTemplate : MessageTemplateModelBase
    {
        [Required]
        public int Id { get; set; }

        public void Load(MessageTemplate template)
        {
            Id = template.Id;
            Name = template.Name;
            Text = template.Text;
            IsActive = template.IsActive;
            MessageEvent = template.MessageEvent;
            OrganizationId = template.OrganizationId;
        }
    }
}