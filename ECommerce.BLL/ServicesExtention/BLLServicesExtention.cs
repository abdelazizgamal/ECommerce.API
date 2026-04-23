using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
namespace ECommerce.BLL
{
    public static class BLLServicesExtention
    {
        public static void AddBLLServices(this IServiceCollection services)
        {
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            //services.AddScoped<IImageManager, ImageManager>();

            services.AddValidatorsFromAssembly(typeof(BLLServicesExtention).Assembly);
            services.AddScoped<IErrorMapper, ErrorMapper>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<ICartManager, CartManager>();

        }
    }
}
