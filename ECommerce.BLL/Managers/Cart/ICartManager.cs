using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface ICartManager
    {
        Task<GeneralResult<CartReadDTO>> GetCartAsync(string userId);
        Task<GeneralResult<CartReadDTO>> AddToCartAsync(string userId, AddToCartDTO addToCartDto);
        Task<GeneralResult<CartReadDTO>> UpdateCartItemAsync(string userId, UpdateCartItemDTO updateCartItemDto);
        Task<GeneralResult<CartReadDTO>> RemoveCartItemAsync(string userId, int productId);
    }
}
