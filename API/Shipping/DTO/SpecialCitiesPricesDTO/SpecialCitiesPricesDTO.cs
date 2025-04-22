using Shipping.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTO.SpecialCitiesPricesDTO
{
    public class SpecialCitiesPriceDTO
    {
        public int? Id { get; set; }

        public int? MerchantId { get; set; }
        public string Government { get; set; }
        public string City { get; set; }
        public int Price { get; set; }

    }
}
