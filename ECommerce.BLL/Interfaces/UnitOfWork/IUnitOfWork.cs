using ECommerce.BLL.Entities;

namespace ECommerce.BLL
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IGenericRepository<Category> Categories { get; }

        IOrderRepository Orders { get; }
        ICartRepository Carts { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<OrderItem> OrderItems { get; }

        Task<int> SaveChangesAsync();
    }
}
