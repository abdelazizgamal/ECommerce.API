using ECommerce.BLL.Entities;
using ECommerce.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser,AppRole, string>
    {
        public AppDbContext(): base()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public override int SaveChanges()
        {
            AuditLog();
            return base.SaveChanges();
        }
        /*------------------------------------------------------------------*/
        private void AuditLog()
        {
            var dateTime = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = dateTime;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = dateTime;
                }
            }
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string connectionString = "Server=.;DataBase=ProductsMangmentDb;Trusted_Connection=true;TrustServerCertificate=true";
        //    optionsBuilder.UseSqlServer(connectionString);
        //    base.OnConfiguring(optionsBuilder);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    }
}
