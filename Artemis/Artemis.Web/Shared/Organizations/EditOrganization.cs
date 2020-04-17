using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.Organizations
{
    public class EditOrganization : OrganizationModelBase
    {
        [Required]
        public int Id { get; set; }
    }
}