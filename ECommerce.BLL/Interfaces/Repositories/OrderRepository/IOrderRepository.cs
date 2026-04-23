using ECommerce.BLL.Entities;

namespace ECommerce.BLL
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllOrdersWithItemsAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderWithItemsAsync(string userId, int orderId);
        Task AddOrderAsync(Order order);
    }
}
