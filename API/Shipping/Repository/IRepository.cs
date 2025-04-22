using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Shipping.Repository.OrderRepo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        
        public Task<List<TEntity>> PaginationAsync(List<TEntity> List, int page = 1, int pageSize = 9);
       
        public Task<List<TEntity>> GetByTitleAsync(string name, params Expression<Func<TEntity, object>>[] includeProperties);
        
        public Task<bool> ExistAsync(int id);
        
        public Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        


        public Task AddAsync(TEntity entity);
      
        public Task UpdateAsync(TEntity entity);
      
        public Task DeleteAsync(int id);
        
        public Task SaveAsync();
       


    }
}
