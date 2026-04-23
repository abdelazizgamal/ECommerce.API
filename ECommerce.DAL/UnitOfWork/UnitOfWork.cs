using ECommerce.BLL;
using ECommerce.BLL.Entities;
namespace ECommerce.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IProductRepository Products { get; }
        public IGenericRepository<Category> Categories { get; }
        public IOrderRepository Orders { get; }
        public ICartRepository Carts { get; }
        public IGenericRepository<CartItem> CartItems { get; }
        public IGenericRepository<OrderItem> OrderItems { get; }

        public UnitOfWork(
            AppDbContext context,
            IProductRepository productRepository,
            IGenericRepository<Category> categoryRepository,
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IGenericRepository<CartItem> cartItemRepository,
            IGenericRepository<OrderItem> orderItemRepository)
        {
            _context = context;

            Products = productRepository;
            Categories = categoryRepository;
            Orders = orderRepository;
            Carts = cartRepository;
            CartItems = cartItemRepository;
            OrderItems = orderItemRepository;
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
