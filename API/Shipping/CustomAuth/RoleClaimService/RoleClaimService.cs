using Microsoft.EntityFrameworkCore;
using Shipping.Models;

namespace Shipping.CustomAuth.RoleClaimService
{
    public class RoleClaimService :  IRoleClaimService
    {
        private readonly ShippingContext _context;

        public RoleClaimService(ShippingContext context)
        {
            _context = context;
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permission)
        {
            // Implement the logic to check if the user has the specified permission
            var userRoles = await _context.UserRoles.Where(ur => ur.UserId == userId).ToListAsync(); 
            var roleClaims = await _context.RoleClaims
                                .Where(rc => userRoles.Select(ur => ur.RoleId).Contains(rc.RoleId) && rc.ClaimValue == permission).ToListAsync();
                               

            return roleClaims.Any();
        }
    }
}
