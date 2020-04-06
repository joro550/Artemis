using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artemis.Web.Server.Data.Models
{
    public class UsAddressEntity : EventAddressEntity
    {
        [Required]
        [Column("PostCode")]
        public string ZipCode { get; set; }

        [Required]
        public string State { get; set; }
    }
}