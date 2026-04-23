using ECommerce.BLL.Entities;
using ECommerce.Common;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddToCartDTO> _addToCartValidator;
        private readonly IValidator<UpdateCartItemDTO> _updateCartItemValidator;
        private readonly IErrorMapper _errorMapper;

        public CartManager(
            IUnitOfWork unitOfWork,
            IValidator<AddToCartDTO> addToCartValidator,
            IValidator<UpdateCartItemDTO> updateCartItemValidator,
            IErrorMapper errorMapper)
        {
            _unitOfWork = unitOfWork;
            _addToCartValidator = addToCartValidator;
            _updateCartItemValidator = updateCartItemValidator;
            _errorMapper = errorMapper;
        }

        public async Task<GeneralResult<CartReadDTO>> GetCartAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetCartWithDetailsByUserIdAsync(userId);

            if (cart is null)
            {
                var emptyCart = new CartReadDTO
                {
                    Items = new List<CartItemReadDTO>(),
                    TotalPrice = 0
                };

                return GeneralResult<CartReadDTO>.SuccessResult(emptyCart);
            }

            return GeneralResult<CartReadDTO>.SuccessResult(MapCartToReadDto(cart));
        }

        public async Task<GeneralResult<CartReadDTO>> AddToCartAsync(string userId, AddToCartDTO addToCartDto)
        {
            var validationResult = await _addToCartValidator.ValidateAsync(addToCartDto);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<CartReadDTO>.FailResult(errors);
            }

            var product = await _unitOfWork.Products.GetByIdAsync(addToCartDto.ProductId);
            if (product is null)
            {
                return GeneralResult<CartReadDTO>.NotFound("Product not found");
            }

            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                await _unitOfWork.Carts.AddCartAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            var cartItem = await _unitOfWork.Carts.GetCartItemAsync(userId, addToCartDto.ProductId);
            if (cartItem is null)
            {
                if (addToCartDto.Quantity > product.Stock)
                {
                    return GeneralResult<CartReadDTO>.FailResult("Quantity exceeds available stock");
                }

                await _unitOfWork.Carts.AddItemAsync(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity
                });
            }
            else
            {
                var totalQuantity = cartItem.Quantity + addToCartDto.Quantity;
                if (totalQuantity > product.Stock)
                {
                    return GeneralResult<CartReadDTO>.FailResult("Quantity exceeds available stock");
                }

                _unitOfWork.Carts.UpdateItemQuantity(cartItem, totalQuantity);
            }

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
            {
                return GeneralResult<CartReadDTO>.FailResult("Failed to update cart");
            }

            var updatedCart = await _unitOfWork.Carts.GetCartWithDetailsByUserIdAsync(userId);
            return GeneralResult<CartReadDTO>.SuccessResult(MapCartToReadDto(updatedCart));
        }

        public async Task<GeneralResult<CartReadDTO>> UpdateCartItemAsync(string userId, UpdateCartItemDTO updateCartItemDto)
        {
            var validationResult = await _updateCartItemValidator.ValidateAsync(updateCartItemDto);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<CartReadDTO>.FailResult(errors);
            }

            var product = await _unitOfWork.Products.GetByIdAsync(updateCartItemDto.ProductId);
            if (product is null)
            {
                return GeneralResult<CartReadDTO>.NotFound("Product not found");
            }

            if (updateCartItemDto.Quantity > product.Stock)
            {
                return GeneralResult<CartReadDTO>.FailResult("Quantity exceeds available stock");
            }

            var cartItem = await _unitOfWork.Carts.GetCartItemAsync(userId, updateCartItemDto.ProductId);
            if (cartItem is null)
            {
                return GeneralResult<CartReadDTO>.NotFound("Cart item not found");
            }

            _unitOfWork.Carts.UpdateItemQuantity(cartItem, updateCartItemDto.Quantity);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
            {
                return GeneralResult<CartReadDTO>.FailResult("Failed to update cart");
            }

            var updatedCart = await _unitOfWork.Carts.GetCartWithDetailsByUserIdAsync(userId);
            return GeneralResult<CartReadDTO>.SuccessResult(MapCartToReadDto(updatedCart));
        }

        public async Task<GeneralResult<CartReadDTO>> RemoveCartItemAsync(string userId, int productId)
        {
            var cartItem = await _unitOfWork.Carts.GetCartItemAsync(userId, productId);
            if (cartItem is null)
            {
                return GeneralResult<CartReadDTO>.NotFound("Cart item not found");
            }

            _unitOfWork.Carts.RemoveItem(cartItem);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
            {
                return GeneralResult<CartReadDTO>.FailResult("Failed to update cart");
            }

            var updatedCart = await _unitOfWork.Carts.GetCartWithDetailsByUserIdAsync(userId);
            if (updatedCart is null)
            {
                return GeneralResult<CartReadDTO>.SuccessResult(new CartReadDTO
                {
                    Items = new List<CartItemReadDTO>(),
                    TotalPrice = 0
                });
            }

            return GeneralResult<CartReadDTO>.SuccessResult(MapCartToReadDto(updatedCart));
        }

        private static CartReadDTO MapCartToReadDto(Cart? cart)
        {
            if (cart is null)
            {
                return new CartReadDTO
                {
                    Items = new List<CartItemReadDTO>(),
                    TotalPrice = 0
                };
            }

            var items = cart.Items
                .Select(i => new CartItemReadDTO
                {
                    ProductId = i.ProductId,
                    ProductTitle = i.Product.Title,
                    ProductPrice = i.Product.Price,
                    ImgUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity,
                    TotalPrice = i.Product.Price * i.Quantity
                })
                .ToList();

            return new CartReadDTO
            {
                Id = cart.Id,
                Items = items,
                TotalPrice = items.Sum(i => i.TotalPrice)
            };
        }
    }
}
