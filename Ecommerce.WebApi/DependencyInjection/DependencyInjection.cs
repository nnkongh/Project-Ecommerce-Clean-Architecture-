using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.WebApi.Interfaces;
using Ecommerce.WebApi.Services;

namespace Ecommerce.WebApi.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAdapterServices(this IServiceCollection services)
        {
            //Register background service
            services.AddScoped<ICartExpirationService, CartExpirationService>();
            services.AddHostedService<CartBackgroundService>();
            return services;
        }
    }
}
