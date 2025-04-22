using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO.AccountDTOs
{
    public class PasswordDTO
    {
        [Required(ErrorMessage = "يجب إدخال الرقم السري القديم")]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "يجب إدخال الرقم السري الجديد")]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
        [Display(Name = "Confirm new password")]
        [Compare("newPassword", ErrorMessage = "الرقم السري غير متطابق")]
        [DataType(DataType.Password)]
        public string confirmNewPassword { get; set; }
    }
}
