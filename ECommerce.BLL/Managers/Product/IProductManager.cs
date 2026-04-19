using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IProductManager
    { /*------------------------------------------------------------------*/

        Task<GeneralResult<PagedResult<ProductReadDTO>>> GetProductsPaginationAsync(PaginationParameters paginationParameters);

        // Get All Products (View Model)
        Task<GeneralResult<IEnumerable<ProductReadDTO>>> GetProductsAsync();
        /*------------------------------------------------------------------*/
        // Get Product By Id (View Model)
        Task<GeneralResult<ProductReadDTO>?> GetProductByIdAsync(int id);
        /*------------------------------------------------------------------*/
        Task<GeneralResult<ProductReadDTO>> CreateProductAsync(ProductCreateDTO productCreateDTO);
        /*------------------------------------------------------------------*/

        Task<int> EditProductAsync(ProductEditDTO productEditVM);
        /*------------------------------------------------------------------*/
        Task<int> DeleteProduct(int id);
        Task<bool> TitleExist(string title);

    }
}
