using Shipping.Models;
using System.ComponentModel.DataAnnotations;
using Shipping.DTO.SpecialCitiesPricesDTO;
namespace Shipping.DTO.MerchantDTOs
{
    public class MerchantDTO
    {
        public string? id { get; set; }

        [Required(ErrorMessage = "يجب ادخال الاسم")]
        public string name { get; set; }

        [Required(ErrorMessage = "يجب ادخال البريد الالكتروني")]
        [RegularExpression("^\\S+@\\S+\\.\\S+$", ErrorMessage = "ادخل بريد الكتروني صحيح")]
        public string email { get; set; }

        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "ادخل رقم هاتف صحيح كالاتى : 01224479550")]
        public string phone { get; set; }

        public bool? status { get; set; } = true;

        [StringLength(255, ErrorMessage = "ادخل ع الاقل 8احرف و ارقام", MinimumLength = 8)]
        public string? password { get; set; }

        [Required(ErrorMessage = "يجب ادخال المحافظة")]
        public string government { get; set; }

        [Required(ErrorMessage = "يجب ادخال المدينة")]
        public string city { get; set; }

        public int pickUpSpecialCost { get; set; }
        public int refusedOrderPercent { get; set; }

        public string? role { get; set; } = "التجار";
        public string? branchName { get; set; } // Change branchId to branchName
        public  List<SpecialCitiesPriceDTO>? SpecialCitiesPrices { get; set; }
    }
}
