using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.Infrastructure.Mapper;
using Ecommerce.Infrastructure.Services;
using Ecommerce.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Dependency_Injection
{
    public static class IdentityInjection
    {
        public static IServiceCollection AddIdentityCore(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequireUppercase = false;
                x.Password.RequiredLength = 6;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireLowercase = true;
            })
               .AddDefaultTokenProviders()
               .AddRoles<IdentityRole>()
               .AddSignInManager()
               .AddEntityFrameworkStores<AppIdentityDbContext>();


            

            /// Figure Identity Token Lifetime
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(2);
            });
            return services;
        }

        
    }
}
