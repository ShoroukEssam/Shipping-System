using Shipping.Models;

namespace Shipping.Repository.BranchRepository
{
    public interface IBranchRepository
    {
        Task<List<Branch>> GetAllAsync();
        Task<Branch> GetByIdAsync(int id);
        Task AddAsync(Branch branch);
        Task UpdateAsync(Branch branch);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<List<Branch>> SearchAsync(string query);
        Task<List<Branch>> GetBranchesByGovernmentNameAsync(int governmentID);
        Task<Branch> GetByNameAsync(string branchName);

    }
}
