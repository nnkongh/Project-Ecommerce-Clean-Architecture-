using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.Infrastructure.Services.Authentication;
using Ecommerce.Infrastructure.Services.ExternalAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Dependency_Injection
{
    public static class ExternalAuthInjection
    {
        public static IServiceCollection AddExternalAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = IdentityConstants.ExternalScheme;
                opt.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
                 {
                     opt.Cookie.HttpOnly = true;
                     opt.LoginPath = "/auth/login";
                     opt.AccessDeniedPath = "/access-denied";
                 })
                 .AddGoogle(GoogleDefaults.AuthenticationScheme,opt =>
                 {
                     opt.ClientId = config["Authentication:Google:ClientId"] ?? throw new Exception("Google ClientId is missing");
                     opt.ClientSecret = config["Authentication:Google:ClientSecret"] ?? throw new Exception("Google ClientSecret is missing");
                     opt.SaveTokens = true;
                     opt.CallbackPath = "/signin-google";

                 });
            services.AddScoped<IExternalLoginService, ExternalLoginService>();
            return services;
        
        }
    }
}

