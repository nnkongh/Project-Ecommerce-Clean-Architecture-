using Ecommerce.WebApi.Interfaces;
using Ecommerce.WebApi.Services;

namespace Ecommerce.WebApi.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAdapterServices(this IServiceCollection services)
        {
            services.AddScoped<ICookieTokenService,CookieTokenService>();
            return services;
        }
    }
}
