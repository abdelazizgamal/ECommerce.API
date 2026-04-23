using ECommerce.BLL;
using ECommerce.BLL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetAllOrdersWithItemsAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithItemsAsync(string userId, int orderId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && o.Id == orderId)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }
    }
}
