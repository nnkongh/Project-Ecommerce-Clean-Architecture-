using Ecommerce.Web.Interface;
using Ecommerce.Web.Services;
using System.Runtime.CompilerServices;

namespace Ecommerce.Web
{
    public static class WebServiceRegister
    {
        public static IServiceCollection AddHttpClientService(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IAuthClient, AuthClient>(client =>
            {
                client.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]!);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            return services;
        }
    }
}
