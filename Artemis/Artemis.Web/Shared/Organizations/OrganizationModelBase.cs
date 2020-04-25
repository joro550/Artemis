using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Shared.Organizations
{
    public class OrganizationModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

    }
}