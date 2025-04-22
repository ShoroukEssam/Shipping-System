using Microsoft.AspNetCore.Identity;
using Shipping.DTO.MerchantDTOs;
using Shipping.Models;

namespace Shipping.Repository.MerchantRepository
{
    public interface IMerchantRepository
    {

        Task<List<Merchant>> GetAllMerchants();
        Task<Merchant> Add(MerchantDTO newMerchant, UserManager<AppUser> _userManager);
        Task<Merchant> GetMerchantByIdAsync(string id);
        List<Merchant> Search(string query);
        Task<Merchant> Update(MerchantDTO newData, UserManager<AppUser> _userManager);
        Task<Merchant> UpdateStatus(Merchant merchant, bool status);
        Task SoftDeleteAsync(Merchant merchant);

    }
}
