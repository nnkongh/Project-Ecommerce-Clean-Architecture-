using Ecommerce.Infrastructure;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Services;
using System.Runtime.CompilerServices;

namespace Ecommerce.Web
{
    public static class WebServiceRegister
    {
        public static IServiceCollection AddHttpClientService(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<AuthTokenHandler>();
            services.AddHttpClient("ApiClient",client =>
            {
                client.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]!);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
                .AddHttpMessageHandler<AuthTokenHandler>();

            services.AddScoped<IAuthClient, AuthClient>();
            services.AddScoped<IProfileService, ProfileService>();

            return services;
        }
    }
}
