using ECommerce.BLL.Entities;
using ECommerce.Common;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CategoryCreateDTO> _categoryCreateValidator;
        private readonly IValidator<CategoryEditDTO> _categoryEditValidator;
        private readonly IErrorMapper _errorMapper;
        public CategoryManager(IUnitOfWork unitOfWork, IValidator<CategoryCreateDTO> categoryCreateValidator, IValidator<CategoryEditDTO> categoryEditValidator, IErrorMapper errorMapper)
        {
            _unitOfWork = unitOfWork;
            _categoryCreateValidator = categoryCreateValidator;
            _categoryEditValidator = categoryEditValidator;
            _errorMapper = errorMapper;
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

        public async Task<GeneralResult<CategoryReadDTO>> CreateCategoryAsync(CategoryCreateDTO categoryCreateDto)
        {
            var validationResult = await _categoryCreateValidator.ValidateAsync(categoryCreateDto);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<CategoryReadDTO>.FailResult(errors);
            }

            var categoryCreate = new Category
            {
                Name = categoryCreateDto.Name
            };
            _unitOfWork.Categories.Insert(categoryCreate);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                return GeneralResult<CategoryReadDTO>.FailResult("Failed to create category");
            }

            var categoryReadDto = new CategoryReadDTO
            {
                Id = categoryCreate.Id,
                Name = categoryCreate.Name
            };

            return GeneralResult<CategoryReadDTO>.SuccessResult(categoryReadDto);
        }

        public async Task<GeneralResult<CategoryReadDTO>> EditCategoryAsync(CategoryEditDTO categoryEditDto)
        {
            var validationResult = await _categoryEditValidator.ValidateAsync(categoryEditDto);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<CategoryReadDTO>.FailResult(errors);
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(categoryEditDto.Id);
            if (category == null)
                return GeneralResult<CategoryReadDTO>.NotFound();

            category.Name = categoryEditDto.Name;
            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                return GeneralResult<CategoryReadDTO>.FailResult("Failed to update category");
            }

            var categoryReadDto = new CategoryReadDTO
            {
                Id = category.Id,
                Name = category.Name
            };

            return GeneralResult<CategoryReadDTO>.SuccessResult(categoryReadDto);
        }

        public async Task<GeneralResult<CategoryReadDTO>> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return GeneralResult<CategoryReadDTO>.NotFound();

            _unitOfWork.Categories.Delete(category);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                return GeneralResult<CategoryReadDTO>.FailResult("Failed to delete category");
            }

            return GeneralResult<CategoryReadDTO>.SuccessResult("Deleted successfully");
        }



    }
}
