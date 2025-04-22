using Shipping.DTO.DeliveryDTOs;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO.OrderDTO
{
    public class OrdersPlusDeliveriesDTO
    {
        [Required(ErrorMessage = "يجب ادخال الطلبات")]
        public List<OrderDTO> Orders { get; set; }
        [Required(ErrorMessage = "يجب ادخال الشحنات")]
         public List<DeliveryDTO> Deliveries { get; set; }
    }
}
