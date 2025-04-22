using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO
{
    public class WeightSettingDTO
    {
        public int? id { get; set; } = 1;
        [Required(ErrorMessage = "يجب إدخال وزن الشحن الافتراضي")]
        public int standaredWeight { get; set; } = 10;


        [Required(ErrorMessage = "يجب إدخال سعر كل كجم إضافي بالجنية")]
        public int addition_Cost { get; set; } = 100;
    }
}
