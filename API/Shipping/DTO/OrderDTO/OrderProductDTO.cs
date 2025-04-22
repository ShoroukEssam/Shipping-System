using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO.OrderDTO
{
    public class OrderProductDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "يجب ادخال اسم المنتج")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "يجب ادخال الكميه")]
        public int ProductQuantity { get; set; }
        [Required(ErrorMessage = "يجب ادخال الوزن")]
        public int Weight { get; set; }
    }
}
