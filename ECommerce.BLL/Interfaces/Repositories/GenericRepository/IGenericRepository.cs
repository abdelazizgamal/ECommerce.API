using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce.BLL
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllGenericAsync
         (
             Expression<Func<T, bool>>? expression = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             bool trackChanges = false,
             params Expression<Func<T, object>>[] includes
         );
        Task <IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetWithIncludeAsync(
        Expression<Func<T, bool>>? predicate = null,
        params Expression<Func<T, object>>[] includes);

        void Insert(T entity);
        void Delete(T entity);
    }
       
    
}
