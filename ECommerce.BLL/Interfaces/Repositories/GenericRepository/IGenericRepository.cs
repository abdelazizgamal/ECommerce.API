using System.Linq.Expressions;

namespace ECommerce.BLL
{
    public interface IGenericRepository<T> where T : class
    {
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
