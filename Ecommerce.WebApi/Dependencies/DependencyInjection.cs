using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.WebApi.Services;

namespace Ecommerce.WebApi.Dependencies
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

        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentityCore();

            services.AddIdentityService();

            services.AddJwtAuthentication(configuration);

            services.AddAdapterServices();

            return services;
        }
    }
}
