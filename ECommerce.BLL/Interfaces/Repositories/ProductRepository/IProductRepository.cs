
using ECommerce.BLL.Entities;
using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        /*------------------------------------------------------------------*/
        Task<Product?> GetByIdWithCategoryAsync(int ProductID);
        /*------------------------------------------------------------------*/
        Task<PagedResult<ProductReadDTO>> GetProductsPagination(PaginationParameters? paginationParameters);
        /*------------------------------------------------------------------*/

        Task<bool> TitleCheck(string title);
    }
}
