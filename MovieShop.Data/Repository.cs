using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MovieShop.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MovieShopDbContext _dbContext;
        public Repository(MovieShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //returning data T which is wrapped using 'Task'
        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        // dont use void method with async, use 'Task' instead
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
                return await _dbContext.Set<T>().Where(filter).CountAsync();
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null)
        {
            return await _dbContext.Set<T>().Where(filter).AnyAsync();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().Where(filter).ToListAsync();
        }
    }
}
