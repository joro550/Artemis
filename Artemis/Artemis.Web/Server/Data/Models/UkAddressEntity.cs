using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Server.Data.Models
{
    public class UkAddressEntity : EventAddressEntity
    {
        [Required]
        public string PostCode { get; set; }

        [Required]
        public string County { get; set; }
    }
}