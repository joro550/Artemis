using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.Organizations
{
    public class CreateOrganization
    {
        [Required]
        public string Name { get; set; }
    }
}