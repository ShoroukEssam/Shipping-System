using CloudinaryDotNet.Actions;
using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [ForeignKey("Government")]
        [Required(ErrorMessage = "يجب ادخال المحافظة")]
        public int? StateId { get; set; }
        [Required(ErrorMessage = "يجب ادخال الاسم الفرع")]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; } = true;
        public bool IsDeleted { get; set; } = false;


        public virtual Government Government { get; set; }
        public virtual List<Order>? Orders { get; set; } = new List<Order>();
        public virtual List<Employee> Employees { get; set; }= new List<Employee>();
        public virtual List<Merchant> Merchants { get; set; } = new List<Merchant>();


    }
}
