using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface ICategoryManager
    {
        Task<GeneralResult<CategoryReadDTO>> CreateCategoryAsync(CategoryCreateDTO categoryCreateDto);
        Task<GeneralResult<CategoryReadDTO>> DeleteCategoryAsync(int id);
        Task<GeneralResult<CategoryReadDTO>> EditCategoryAsync(CategoryEditDTO categoryEditDto);
        Task<IEnumerable<CategoryReadDTO>> GetCategoriesAsync();
        Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
        /*------------------------------------------------------------------*/

    }
}
