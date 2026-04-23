using ECommerce.BLL.Entities;

namespace ECommerce.BLL
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);

        Task<Cart?> GetCartWithDetailsByUserIdAsync(string userId);

        Task<CartItem?> GetCartItemAsync(string userId, int productId);

        Task AddCartAsync(Cart cart);

        Task AddItemAsync(CartItem cartItem);

        void UpdateItemQuantity(CartItem cartItem, int quantity);

        void RemoveItem(CartItem cartItem);
    }
}