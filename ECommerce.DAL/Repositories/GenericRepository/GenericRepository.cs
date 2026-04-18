using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ECommerce.BLL;

namespace ECommerce.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetWithIncludeAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable<T>();

            if(predicate != null)
                query = query.Where(predicate);
            if(includes != null)
                foreach(var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public void Insert(T entity)
        {
            _context.Set<T>().AddAsync(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

    }
}
