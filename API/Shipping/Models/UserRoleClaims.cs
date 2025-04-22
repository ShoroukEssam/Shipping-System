using Microsoft.AspNetCore.Identity;

namespace Shipping.Models
{
    public class UserRoleClaims : IdentityRoleClaim<string>
    {
        public string? ArabicName { get; set; }
    }
}
