using ECommerce.BLL;
using ECommerce.BLL.Entities;
using ECommerce.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int ProductID)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == ProductID);
        }

        public async Task<PagedResult<ProductReadDTO>> GetProductsPagination(PaginationParameters? paginationParameters,
            ProductFilterParameters? productFilterParameters)
        {
            IQueryable<Product> query = _context.Set<Product>().AsQueryable();

            query = query.Include(p => p.Category);


            //Filtering
            if (productFilterParameters != null)
            {
                query = ApplyFilter(query, productFilterParameters);
            }


            // Pagination
            var totalCount = await query.CountAsync();

            var pageNumber = paginationParameters?.PageNumber ?? 1;
            var pageSize = paginationParameters?.PageSize ?? totalCount;

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<ProductReadDTO>
            {
                Items = items.Select(e => new ProductReadDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Price = e.Price,
                    Count = e.Stock,
                    Category = e.Category.Name,
                    ImgUrl = e.ImageUrl
                }).ToList(),
                Metadata = new PaginationMetadata
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasNext = pageNumber < totalPages,
                    HasPrevious = pageNumber > totalPages,
                }
            };

        }
        public async Task<bool> TitleCheck(string title)
        {
            return await _context.Products.AnyAsync(p => p.Title == title);
        }

        private IQueryable<Product> ApplyFilter(
    IQueryable<Product> query,
    ProductFilterParameters productFilterParameters)
        {
            if (productFilterParameters.MinPrice > 0)
            {
                query = query.Where(p => p.Price >= productFilterParameters.MinPrice);
            }

            if (productFilterParameters.MaxPrice > 0)
            {
                query = query.Where(p => p.Price <= productFilterParameters.MaxPrice);
            }

            if (productFilterParameters.MinStock > 0)
            {
                query = query.Where(p => p.Stock >= productFilterParameters.MinStock);
            }

            if (productFilterParameters.MaxStock > 0)
            {
                query = query.Where(p => p.Stock <= productFilterParameters.MaxStock);
            }

            if (productFilterParameters.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == productFilterParameters.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(productFilterParameters.Search))
            {
                var search = productFilterParameters.Search.Trim();
                query = query.Where(p =>
                    p.Title.Contains(search) ||
                    p.Description.Contains(search));
            }

            return query;
        }

    }
}