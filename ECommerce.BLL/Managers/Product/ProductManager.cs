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
        private readonly IErrorMapper _errorMapper;
        public ProductManager(IUnitOfWork unitOfWork, IValidator<ProductCreateDTO> validator, IErrorMapper errorMapper)
        {
            _unitOfWork = unitOfWork;
            _productCreateValidator = validator;
            _errorMapper = errorMapper;
        }

        public async Task<GeneralResult<PagedResult<ProductReadDTO>>> GetProductsPaginationAsync
            (
                PaginationParameters paginationParameters
            )
        {
            var pagedResult = await _unitOfWork.Products.GetProductsPagination(paginationParameters);
            
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

        public async Task<int> EditProductAsync(ProductEditDTO productEditDTO)
        {
            var productUpdate = await _unitOfWork.Products.GetByIdAsync(productEditDTO.Id);

            if (productUpdate == null)
            {
                return 0;
            }



            productUpdate.Title = productEditDTO.Title;
            productUpdate.Description = productEditDTO.Description!;
            productUpdate.Price = productEditDTO.Price;
            productUpdate.Stock = productEditDTO.Count;
            productUpdate.CategoryId = productEditDTO.CategoryId;

          

            return await _unitOfWork.SaveChangesAsync();
        }
        public async Task<int> DeleteProduct(int productId)
        {
            var productDelete = await _unitOfWork.Products.GetByIdAsync(productId);

            if (productDelete == null)
            {
                return 0;
            }
            _unitOfWork.Products.Delete(productDelete);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> TitleExist(string title)
        {
            return await _unitOfWork.Products.TitleCheck(title);
        }

       
    }
}
