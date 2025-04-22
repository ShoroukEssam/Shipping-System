using Microsoft.AspNetCore.Identity;

namespace Shipping.Models
{
    public class UserRole : IdentityRole<string>
    {
        public string? Date { get; set; }
    }
}
