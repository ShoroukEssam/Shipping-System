using Microsoft.AspNetCore.Identity;

namespace Shipping.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Status { get; set; } = true;


        public virtual List<Employee>? Employees { get; set; }=new List<Employee>();

        public virtual List<Merchant>? Merchants { get; set; }=new List<Merchant>();

        public virtual List<Delivery>? Deliveries { get; set; } = new List<Delivery>();
    }



}
