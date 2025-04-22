using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        public int PickUpSpecialCost { get; set; }
        public int RefusedOrderPercent { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { set; get; }


        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }


        [ForeignKey("City")]
        public int? CityId { get; set; }
        public virtual City? City { get; set; }


        [ForeignKey("Government")]
        public int? GovernmentId { get; set; }
        public virtual Government? Government { get; set; }

        public virtual List<SpecialCitiesPrice>? SpecialCitiesPrices { get; set; } = new List<SpecialCitiesPrice>();
    }
}
