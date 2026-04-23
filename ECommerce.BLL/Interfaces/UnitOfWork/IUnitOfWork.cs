using ECommerce.BLL.Entities;

namespace ECommerce.BLL
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IGenericRepository<Category> Categories { get; }

        IGenericRepository<Order> Orders { get; }
        ICartRepository Carts { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<OrderItem> OrderItems { get; }

        Task<int> SaveChangesAsync();
    }
}
