namespace ECommerce.BLL
{
    public interface ICategoryManager
    {
        Task<int> CreateCategoryAsync(CategoryCreateDTO categoryCreateDto);
        Task<int> DeleteCategoryAsync(int id);
        Task<int> EditCategoryAsync(CategoryEditDTO categoryEditDto);
        Task<IEnumerable<CategoryReadDTO>> GetCategoriesAsync();
        Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
        /*------------------------------------------------------------------*/

    }
}
