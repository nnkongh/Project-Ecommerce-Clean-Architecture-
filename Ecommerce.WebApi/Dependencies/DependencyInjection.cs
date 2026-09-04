using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.Infrastructure.Services;
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
            services.AddPhotoService(configuration);
            services.AddCors();
            services.AddSignalRService();
            return services;
        }
        public static IServiceCollection AddPhotoService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPhotoService, PhotoService>();
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            return services;
        }
        public static IServiceCollection AddSignalRService(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddScoped<INotificationService,NotificationService>();
            return services;
        }
        public static IServiceCollection AddCORS(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("SignalRPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:7214")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            return services;
        }
    }
}
