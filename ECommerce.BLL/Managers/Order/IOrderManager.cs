using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IOrderManager
    {
        Task<GeneralResult<OrderReadDTO>> CreateOrderAsync(string userId, CreateOrderDTO? createOrderDto = null);
        Task<GeneralResult<IEnumerable<OrderReadDTO>>> GetAllOrdersAsync();
        Task<GeneralResult<IEnumerable<OrderReadDTO>>> GetMyOrdersAsync(string userId);
        Task<GeneralResult<OrderReadDTO>> GetOrderByIdAsync(string userId, int orderId);
    }
}
