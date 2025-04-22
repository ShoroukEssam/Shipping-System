using Shipping.Models;

namespace Shipping.Repository.ArabicNamesForRoleClaims
{
    public interface IAddArabicNamesForRoleClaims
    {
        bool AddArabicNamesToRoleCaims(UserRole role, string ArabicName, string claimValue);
    }
}
