using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO
{
    public class UserRoleDTO
    {
        public string? id { get; set; }

        public string? date { get; set; }

        [Required(ErrorMessage = "يجب إدخال اسم المجموعه")]
        public string roleName { get; set; }
    }
}
