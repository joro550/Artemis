using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Artemis.Web.Shared.EventAddresses;

namespace Artemis.Web.Server.Data.Models
{
    [Table("EventAddress")]
    public class EventAddressEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public string FirstLine { get; set; }

        [Required]
        public string SecondLine { get; set; }

        [Required]
        public string ThirdLine { get; set; }

        [Required]
        public AddressType AddressType { get; set; }
    }
}