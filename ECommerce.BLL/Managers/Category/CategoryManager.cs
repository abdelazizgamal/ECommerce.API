using ECommerce.BLL.Entities;

namespace ECommerce.BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<CategoryReadDTO>> GetCategoriesAsync()
        {
            var catigories = await _unitOfWork.Categories.GetAllAsync();
            var categoryReadDto = catigories
                    .Select(c => new CategoryReadDTO
                    {
                        Id = c.Id,
                        Name = c.Name
                    });
            return categoryReadDto;
        }
        public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
        {
            var categoryReadDto = await _unitOfWork.Categories.GetByIdAsync(id);
            if (categoryReadDto == null)
            { return null; }

            return new CategoryReadDTO { Id = categoryReadDto.Id, Name = categoryReadDto.Name };
        }

        public async Task<int> CreateCategoryAsync(CategoryCreateDTO categoryCreateDto)
        {
            var categoryCreate = new Category
            {
                Name = categoryCreateDto.Name
            };
            _unitOfWork.Categories.Insert(categoryCreate);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> EditCategoryAsync(CategoryEditDTO categoryEditDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryEditDto.Id);
            if (category == null)
                return 0;
            category.Name = categoryEditDto.Name;
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return 0;
            _unitOfWork.Categories.Delete(category);
            return await _unitOfWork.SaveChangesAsync();
        }



    }
}
