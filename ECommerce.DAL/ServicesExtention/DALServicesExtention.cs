using ECommerce.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.DAL.Seed;
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
        }
    }
}
