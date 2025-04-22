using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO.OrderDTO
{
    public class OrderDTO
    {
        public int? Id { get; set; }

       [Required(ErrorMessage = "يجب ادخال الاسم")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        public int ClientPhoneNumber1 { get; set; }

        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        public int? ClientPhoneNumber2 { get; set; }

        [Required(ErrorMessage = "يجب ادخال البريد الالكتروني")]
        [RegularExpression("^\\S+@\\S+\\.\\S+$", ErrorMessage = "ادخل بريد الكتروني صحيح")]
        public string ClientEmail { get; set; }

        [Required(ErrorMessage = "يجب ادخال التكلفه")]
        public int OrderCost { get; set; }

        [Required(ErrorMessage = "يجب ادخال الوزن الكلي")]
        public int TotalWeight { get; set; }

        [Required(ErrorMessage = "يجب ادخال هل الشحن لقاريه ام لا ")]
        public bool IsVillage { get; set; }

        public string? StateName { get; set; }

        [Required(ErrorMessage = "يجب ادخال المدينه")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "يجب ادخال الفرع")]
        public string BranchName { get; set; }

        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "يجب ادخال اسم الشارع")]
        public string StreetName { get; set; }

        public string? Notes { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required(ErrorMessage = "يجب ادخال تكلفه الشحن")]
        public int ShippingCost { get; set; }

        public int? TotalCost { get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }

        public int? BranchId { get; set; }
        public string? DeliveryId { get; set; }
        public int? MerchantId { get; set; }

        [Required(ErrorMessage = "يجب ادخال النوع ")]
        public Models.Type Type { get; set; }

        [Required(ErrorMessage = "يجب ادخال نوع الشحن")]
        public ShippingType ShippingType { get; set; }

        [Required(ErrorMessage = "يجب ادخال طريقه الدفع")]
        public PaymentType PaymentType { get; set; }

        [Required(ErrorMessage = "يجب ادخال حاله الطلب")]
        public string OrderStatus { get; set; } = "جديد";



    }
}

