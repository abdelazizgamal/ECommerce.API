using ECommerce.BLL;
using ECommerce.BLL.Entities;
using ECommerce.DAL.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ECommerce.DAL
{
    public static class DALServicesExtention
    {
        public static void AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ECommerce");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString)
                .UseAsyncSeeding(async (context, _, _) =>
                {
                    await SeedRunner.RunAsync(context);
                })
                .UseSeeding((context, _) =>
                {
                    SeedRunner.RunAsync(context).GetAwaiter().GetResult();
                });
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
