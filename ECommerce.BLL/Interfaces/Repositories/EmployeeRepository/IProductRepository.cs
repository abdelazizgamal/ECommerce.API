
using ECommerce.BLL.Entities;
using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        /*------------------------------------------------------------------*/
        Task<Product?> GetByIdWithCategoryAsync(int ProductID);
        /*------------------------------------------------------------------*/
        Task<PagedResult<Product>> GetProductsPagination(PaginationParameters? paginationParameters);
        /*------------------------------------------------------------------*/
    }
}
