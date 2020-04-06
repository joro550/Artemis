using System.ComponentModel.DataAnnotations;

namespace Artemis.Web.Server.Data.Models
{
    public class UsAddressEntity : EventAddressEntity
    {
        [Required]
        public string PostCode { get; set; }

        [Required]
        public string State { get; set; }
    }
}