using Shipping.Models;

namespace Shipping.Repository.ArabicNamesForRoleClaims
{
    public class AddArabicNamesForRoleClaims : IAddArabicNamesForRoleClaims
    {
        private ShippingContext context;
        public AddArabicNamesForRoleClaims(ShippingContext _context)
        {
            context= _context;
        }
        public bool AddArabicNamesToRoleCaims(UserRole role, string ArabicName, string claimValue)
        {
            var roleClaim = context.RoleClaims.FirstOrDefault(rc => rc.RoleId == role.Id && rc.ClaimValue == claimValue);
            if (roleClaim == null) return false;

            roleClaim.ArabicName = ArabicName;
            context.SaveChanges();
            return true; 
        }
    }
}
