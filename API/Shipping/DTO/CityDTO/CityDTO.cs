using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO.CityDTO
{
    public class CityDTO
    {
        public int? id { get; set; }
        [Required(ErrorMessage = "يجب ادخال الاسم")]
        public string name { get; set; }
        [Required(ErrorMessage = "يجب ادخال سعر الشحن")]
        public int shippingPrice { get; set; }
        [Required(ErrorMessage = "يجب ادخال سعر الاستلام")]
        public int pickUpPrice { get; set; }
        public bool? status { get; set; } = true;
        public int? governmentId { get; set; }
        public string? governmentName { get; set; }
    }
}
