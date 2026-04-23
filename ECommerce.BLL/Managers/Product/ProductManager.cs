using ECommerce.BLL.Entities;
using ECommerce.Common;
using FluentValidation;
using System.Threading.Tasks;

namespace ECommerce.BLL
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProductCreateDTO> _productCreateValidator;
        private readonly IValidator<ProductEditDTO> _productEditValidator;
        private readonly IErrorMapper _errorMapper;
        public ProductManager(IUnitOfWork unitOfWork, IValidator<ProductCreateDTO> validator, IValidator<ProductEditDTO> productEditValidator, IErrorMapper errorMapper)
        {
            _unitOfWork = unitOfWork;
            _productCreateValidator = validator;
            _productEditValidator = productEditValidator;
            _errorMapper = errorMapper;
        }

        public async Task<GeneralResult<PagedResult<ProductReadDTO>>> GetProductsPaginationAsync
            (
                PaginationParameters paginationParameters, ProductFilterParameters? productFilterParameters = null
            )
        {
            var pagedResult = await _unitOfWork.Products.GetProductsPagination(paginationParameters, productFilterParameters);

            return GeneralResult<PagedResult<ProductReadDTO>>.SuccessResult(pagedResult);
        }

        public async Task<GeneralResult<IEnumerable<ProductReadDTO>>> GetProductsAsync()
        {
            var products = await _unitOfWork.Products.GetWithIncludeAsync(null, p => p.Category);

            var productsReadDto = products
                .Select(p => new ProductReadDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Count = p.Stock,
                    Category = p.Category.Name,
                    ImgUrl = p.ImageUrl

                });
            return GeneralResult<IEnumerable<ProductReadDTO>>.SuccessResult(productsReadDto);
        }

        public async Task<GeneralResult<ProductReadDTO>?> GetProductByIdAsync(int id)
        {
            var p = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
            if (p is null)
                return GeneralResult<ProductReadDTO>.NotFound();


            var productReadDTO = new ProductReadDTO
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Count = p.Stock,
                Category = p.Category.Name,
                ImgUrl = p.ImageUrl

            };
            return GeneralResult<ProductReadDTO>.SuccessResult(productReadDTO);
        }
        public async Task<GeneralResult<ProductReadDTO>> CreateProductAsync(ProductCreateDTO productCreateDTO)
        { 
            var validationResult = await _productCreateValidator.ValidateAsync(productCreateDTO);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<ProductReadDTO>.FailResult(errors);
            }

            var cat = await _unitOfWork.Categories.GetByIdAsync(productCreateDTO.CategoryId);
            if (cat == null)
            {
                return GeneralResult<ProductReadDTO>.FailResult("Category not found");
            }
            var p = new Product
            {
                Title = productCreateDTO.Title,
                Description = productCreateDTO.Description!,
                Price = productCreateDTO.Price,
                Stock = productCreateDTO.Count,
                CategoryId = productCreateDTO.CategoryId,
                ImageUrl = productCreateDTO.ImgUrl
            };

            _unitOfWork.Products.Insert(p);
            var result = await _unitOfWork.SaveChangesAsync();
            

            if (result > 0)
            {
                var productReadDTO = new ProductReadDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description!,
                    Price = p.Price,
                    Count = p.Stock,
                    Category = cat.Name,
                    ImgUrl = p.ImageUrl
                };
                return GeneralResult<ProductReadDTO>.SuccessResult(productReadDTO);
            }
            return GeneralResult<ProductReadDTO>.FailResult("Failed to create product");
        }

        public async Task<GeneralResult<ProductReadDTO>> EditProductAsync(ProductEditDTO productEditDTO)
        {
            var validationResult = await _productEditValidator.ValidateAsync(productEditDTO);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<ProductReadDTO>.FailResult(errors);
            }

            var productUpdate = await _unitOfWork.Products.GetByIdAsync(productEditDTO.Id);

            if (productUpdate == null)
            {
                return GeneralResult<ProductReadDTO>.NotFound();
            }

            var cat = await _unitOfWork.Categories.GetByIdAsync(productEditDTO.CategoryId);
            if (cat == null)
            {
                return GeneralResult<ProductReadDTO>.FailResult("Category not found");
            }


            productUpdate.Title = productEditDTO.Title;
            productUpdate.Description = productEditDTO.Description!;
            productUpdate.Price = productEditDTO.Price;
            productUpdate.Stock = productEditDTO.Count;
            productUpdate.CategoryId = productEditDTO.CategoryId;
            productUpdate.ImageUrl = productEditDTO.ImgUrl;

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
            {
                return GeneralResult<ProductReadDTO>.FailResult("Failed to update product");
            }

            var productReadDTO = new ProductReadDTO
            {
                Id = productUpdate.Id,
                Title = productUpdate.Title,
                Description = productUpdate.Description,
                Price = productUpdate.Price,
                Count = productUpdate.Stock,
                Category = cat.Name,
                ImgUrl = productUpdate.ImageUrl
            };

            return GeneralResult<ProductReadDTO>.SuccessResult(productReadDTO);
        }
        public async Task<GeneralResult<ProductReadDTO>> DeleteProduct(int productId)
        {
            var productDelete = await _unitOfWork.Products.GetByIdAsync(productId);

            if (productDelete == null)
            {
                return GeneralResult<ProductReadDTO>.NotFound();
            }
            _unitOfWork.Products.Delete(productDelete);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
            {
                return GeneralResult<ProductReadDTO>.FailResult("Failed to delete product");
            }

            return GeneralResult<ProductReadDTO>.SuccessResult("Deleted successfully");
        }

        public async Task<bool> TitleExist(string title)
        {
            return await _unitOfWork.Products.TitleCheck(title);
        }

       
    }
}
