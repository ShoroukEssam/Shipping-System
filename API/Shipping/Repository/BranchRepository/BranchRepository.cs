using Microsoft.EntityFrameworkCore;
using Shipping.Models;

namespace Shipping.Repository.BranchRepository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ShippingContext _context;

        public BranchRepository(ShippingContext context)
        {
            _context = context;
        }

        public async Task<List<Branch>> GetAllAsync()
        {
            return await _context.Branches.ToListAsync();
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            return await _context.Branches.FindAsync(id);
        }

        public async Task AddAsync(Branch branch)
        {
           
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Branch branch)
        {
            _context.Entry(branch).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Branches.AnyAsync(e => e.Id == id);
        }
        #region Search For Branch
        public async Task<List<Branch>> SearchAsync(string query)
        {
            return await _context.Branches
                .Include(b => b.Government)
                .Where(b => b.Name.Contains(query) || b.Government.Name.Contains(query))
                .ToListAsync();
        }
        #endregion
        #region GetBranchesByGovernmentName
        public async Task<List<Branch>> GetBranchesByGovernmentNameAsync(int governmentID)
        {
            var branches = await _context.Branches.Where(b=>b.StateId==governmentID).ToListAsync();
            return branches;
        }

        public Task<Branch> GetByNameAsync(string branchName)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
