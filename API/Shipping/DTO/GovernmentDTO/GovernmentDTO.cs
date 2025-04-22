using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO.GovernmentDTO
{
    public class GovernmentDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "يجب ادخال الاسم")]
        public string Name { get; set; }
        public bool? Status { get; set; } = true;

    }
}
