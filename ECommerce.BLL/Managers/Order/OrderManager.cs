using ECommerce.BLL.Entities;
using ECommerce.Common;

namespace ECommerce.BLL
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a new order from the current user cart.
        /// </summary>
        public async Task<GeneralResult<OrderReadDTO>> CreateOrderAsync(string userId, CreateOrderDTO? createOrderDto = null)
        {
            var cart = await _unitOfWork.Carts.GetCartWithDetailsByUserIdAsync(userId);
            if (cart is null || !cart.Items.Any())
            {
                return GeneralResult<OrderReadDTO>.FailResult("Cart is empty");
            }

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                ShippingCountry = string.IsNullOrWhiteSpace(createOrderDto?.ShippingCountry) ? "Not Provided" : createOrderDto.ShippingCountry,
                ShippingCity = string.IsNullOrWhiteSpace(createOrderDto?.ShippingCity) ? "Not Provided" : createOrderDto.ShippingCity,
                Items = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var cartItem in cart.Items)
            {
                var product = cartItem.Product ?? await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId);
                if (product is null)
                {
                    return GeneralResult<OrderReadDTO>.NotFound("Product not found");
                }

                if (cartItem.Quantity > product.Stock)
                {
                    return GeneralResult<OrderReadDTO>.FailResult($"Insufficient stock for product {product.Title}");
                }

                product.Stock -= cartItem.Quantity;

                order.Items.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPriceAtPurchase = product.Price
                });

                totalAmount += product.Price * cartItem.Quantity;
            }

            order.TotalAmount = totalAmount;

            await _unitOfWork.Orders.AddOrderAsync(order);

            _unitOfWork.CartItems.DeleteRange(cart.Items);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
            {
                return GeneralResult<OrderReadDTO>.FailResult("Failed to create order");
            }

            var createdOrder = await _unitOfWork.Orders.GetOrderWithItemsAsync(userId, order.Id);
            return GeneralResult<OrderReadDTO>.SuccessResult(MapOrderToReadDto(createdOrder ?? order));
        }

        /// <summary>
        /// Gets all orders for admin users.
        /// </summary>
        public async Task<GeneralResult<IEnumerable<OrderReadDTO>>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllOrdersWithItemsAsync();
            var ordersReadDto = orders.Select(MapOrderToReadDto);

            return GeneralResult<IEnumerable<OrderReadDTO>>.SuccessResult(ordersReadDto);
        }

        /// <summary>
        /// Gets all orders for the current user.
        /// </summary>
        public async Task<GeneralResult<IEnumerable<OrderReadDTO>>> GetMyOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
            var ordersReadDto = orders.Select(MapOrderToReadDto);

            return GeneralResult<IEnumerable<OrderReadDTO>>.SuccessResult(ordersReadDto);
        }

        /// <summary>
        /// Gets order details for the current user.
        /// </summary>
        public async Task<GeneralResult<OrderReadDTO>> GetOrderByIdAsync(string userId, int orderId)
        {
            var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(userId, orderId);
            if (order is null)
            {
                return GeneralResult<OrderReadDTO>.NotFound();
            }

            return GeneralResult<OrderReadDTO>.SuccessResult(MapOrderToReadDto(order));
        }

        private static OrderReadDTO MapOrderToReadDto(Order order)
        {
            return new OrderReadDTO
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                Status = order.Status.ToString(),
                Items = order.Items.Select(i => new OrderItemReadDTO
                {
                    ProductId = i.ProductId,
                    ProductTitle = i.Product?.Title ?? string.Empty,
                    UnitPrice = i.UnitPriceAtPurchase,
                    Quantity = i.Quantity,
                    TotalPrice = i.UnitPriceAtPurchase * i.Quantity
                }).ToList()
            };
        }
    }
}
