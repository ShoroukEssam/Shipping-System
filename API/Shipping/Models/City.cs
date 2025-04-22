using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class City
    {
        public int Id { get; set; }
        [ForeignKey("Government")]
        public int GovernmentId { get; set; }
        public string Name { get; set; }
        public int ShippingPrice { get; set; }
        public int PickUpPrice { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Status { get; set; } = true;

        public virtual Government? Government { get; set; }
    }
}
