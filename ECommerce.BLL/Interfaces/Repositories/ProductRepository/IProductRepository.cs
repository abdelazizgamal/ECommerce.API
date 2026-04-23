
using ECommerce.BLL.Entities;
using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        /*------------------------------------------------------------------*/
        Task<Product?> GetByIdWithCategoryAsync(int ProductID);
        /*------------------------------------------------------------------*/
        Task<PagedResult<ProductReadDTO>> GetProductsPagination(PaginationParameters? paginationParameters,
            ProductFilterParameters? productFilterParameters);
        /*------------------------------------------------------------------*/

        Task<bool> TitleCheck(string title);
    }
}
