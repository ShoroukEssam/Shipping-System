using Shipping.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Linq.Expressions;

namespace Shipping.Repository.OrderRepo
{
    public class Repository<TEntity> :IRepository<TEntity> where TEntity : class
    {
        ShippingContext db;
        DbSet<TEntity> set; 
        public Repository(ShippingContext db)
        {
            this.db = db;
            set = this.db.Set<TEntity>();
        }
        public async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = set;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToList();
        }
        public async Task<List<TEntity>> PaginationAsync(List<TEntity> List, int page = 1, int pageSize = 9)
        {
            IQueryable<TEntity> query = set;
            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<List<TEntity>> GetByTitleAsync(string name, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = set;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where(entity => EF.Property<string>(entity, "Title") == name).ToList();
        }
        public async Task<bool> ExistAsync(int id)
        {
            IQueryable<TEntity> query = set;
            var entityIdProperty = typeof(TEntity).GetProperty("Id");
            if (entityIdProperty != null)
            {
                var parameter = Expression.Parameter(typeof(TEntity), "entity");
                var idProperty = Expression.Property(parameter, entityIdProperty);
                var lambda = Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(idProperty, Expression.Constant(id)),
                    parameter);
                return query.Any(lambda);
            }
            throw new InvalidOperationException("Entity does not contain properties named 'Id'.");
        }
        public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = set;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            var entityIdProperty = typeof(TEntity).GetProperty("Id");
            if (entityIdProperty != null)
            {
                var parameter = Expression.Parameter(typeof(TEntity), "entity");
                var idProperty = Expression.Property(parameter, entityIdProperty);
                var lambda = Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(idProperty, Expression.Constant(id)),
                    parameter);
                return query.FirstOrDefault(lambda);
            }
            throw new InvalidOperationException("Entity does not contain properties named 'Id'.");
        }

        public async Task AddAsync(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }
        public async Task UpdateAsync(TEntity entity)
        {
            db.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public async Task DeleteAsync(int id)
        {
            TEntity obj = db.Set<TEntity>().Find(id);
            db.Set<TEntity>().Remove(obj);
        }
        public async Task SaveAsync()
        {
            db.SaveChanges();
        }


    }
}
