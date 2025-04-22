using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO.AccountDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "يجب ادخال البريد الالكتروني او اسم المستخدم")]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "يجب ادخال الرقم السري")]
        public string password { get; set; }
        public bool rememberMe { get; set; }
    }
}
