using CloudinaryDotNet;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.Infrastructure.Services;
using Ecommerce.Infrastructure.Services.Authentication;
using Ecommerce.Web.Authorization;
using Ecommerce.Web.Client.Strategy;
using Ecommerce.Web.Features.Authentication;
using Ecommerce.Web.Features.Carts;
using Ecommerce.Web.Handlers;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Mapping;
using Ecommerce.Web.Services;
using Ecommerce.Web.Services.Strategy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

            services.AddAuthentication(configuration);

            services.AddHttpClientService(configuration);

           // services.AddApplicationAuthentication();

            services.AddCookieToken();

            services.AddPhotoService(configuration);

            services.AddHttpClientService(configuration);

            services.AddCartSession();

            services.AddScoped<IPrincipalFactory, PrincipalFactory>();

            return services;
        }
        public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            return services;
        }

        public static IServiceCollection AddPolicyBase(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, AuthorizationHandler<OrderOwnerRequirement>>();

            services.AddAuthorization(opt =>
            {
                // Check role 
                opt.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

                opt.AddPolicy("UserOnly", policy => policy.RequireRole("User","Admin"));

                opt.AddPolicy("OrderOnwer", policy => policy.Requirements.Add(new OrderOwnerRequirement()));
            });
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
            services.AddScoped<IProductClient, ProductClient>();
            services.AddScoped<IOrderClient, OrderClient>();
            services.AddScoped<ICategoryClient, CategoryClient>();
            services.AddScoped<ICommentClient, CommentClient>();
            services.AddScoped<IWishlistClient, WishlistClient>();
            services.AddScoped<ICheckoutCartClient,CheckoutCartClient>();

            services.AddScoped<CartApiClient>();
            services.AddScoped<CartSessionClient>();
            services.AddScoped<CartService>();
            services.AddScoped<ICartStrategyFactory, CartStrategyFactory>();

            return services;
        }
        public static IServiceCollection AddPhotoService(this IServiceCollection services, IConfiguration config)
        {

            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));

            services.AddScoped<IPhotoService, PhotoService>();

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
        public static IServiceCollection AddCartSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromDays(7);
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
            });
            return services;
        }
    }
}
