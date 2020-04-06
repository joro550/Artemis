using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artemis.Web.Server.Data.Models
{
    public class UkAddressEntity : EventAddressEntity
    {
        [Required]
        [Column("PostCode")]
        public string PostCode { get; set; }

        [Required]
        public string County { get; set; }
    }
}