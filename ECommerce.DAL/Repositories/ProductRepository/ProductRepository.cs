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

        public async Task<PagedResult<ProductReadDTO>> GetProductsPagination(PaginationParameters? paginationParameters)
        {
            IQueryable<Product> query = _context.Set<Product>().AsQueryable();

            query = query.Include(p => p.Category);

          

            // Total Count
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

    }
}
