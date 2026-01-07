using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.Infrastructure.Services.Authentication;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Mapping;
using Ecommerce.Web.Services;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Ecommerce.Web.Dependencies
{
    public static class WebServicesRegister
    {
        public static IServiceCollection AddWebMvcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(ViewModelsMapping));

            services.AddIdentityService();

            services.AddIdentity();

            services.AddExternalAuthentication(configuration);

            services.AddHttpClientService(configuration);

            services.AddCookieToken();

            services.AddHttpClientService(configuration);

            services.AddScoped<IPrincipalFactory, PrincipalFactory>();

            return services;
        }
        public static IServiceCollection AddHttpClientService(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();

            services.AddTransient<AuthTokenHandler>();

            services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]!);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
                .AddHttpMessageHandler<AuthTokenHandler>();

            services.AddScoped<IAuthenticationClient, AuthenticationClient>();
            services.AddScoped<IProfileClient, ProfileClient>();
            services.AddScoped<ICartClient, CartClient>();
            services.AddScoped<IProductClient, ProductClient>();
            services.AddScoped<ICategoryClient, CategoryClient>();
            services.AddScoped<ICommentClient, CommentClient>();
            services.AddScoped<IWishlistClient, WishlistClient>();
            return services;
        }

        //AddIdentity tự động đăng ký các scheme như ApplicationScheme, ExternalScheme, TwoFactorRememberMeScheme
        // Khi sử dụng addsigninmanager, thì asp.net identity sẽ dùng các scheme mặc định khi gọi các method như
        // getexternallogininfoasync và externalloginsigninasync

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                   .AddDefaultTokenProviders()
                   .AddSignInManager()
                   .AddEntityFrameworkStores<AppIdentityDbContext>();

            return services;
        }

        public static IServiceCollection AddCookieToken(this IServiceCollection services)
        {
            services.AddScoped<ICookieTokenService, CookieTokenService>();

            return services;
        }
    }
}
