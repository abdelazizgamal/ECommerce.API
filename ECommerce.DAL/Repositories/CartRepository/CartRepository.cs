using ECommerce.BLL;
using ECommerce.BLL.Entities;
using ECommerce.DAL;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetCartWithDetailsByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem?> GetCartItemAsync(string userId, int productId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task AddItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        public void UpdateItemQuantity(CartItem cartItem, int quantity)
        {
            cartItem.Quantity = quantity;
        }

        public void RemoveItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }
    }
}