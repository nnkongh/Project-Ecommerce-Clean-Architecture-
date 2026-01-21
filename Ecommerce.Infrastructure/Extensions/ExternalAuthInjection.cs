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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
                {
                    opt.Cookie.HttpOnly = true;
                    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    opt.Cookie.SameSite = SameSiteMode.Lax;
                    opt.AccessDeniedPath = "/auth/login";
                    opt.LoginPath = "/auth/login"; // Nếu cần
                    opt.LogoutPath = "/auth/logout"; // Nếu cần
                })
                 .AddGoogle(GoogleDefaults.AuthenticationScheme,opt =>
                 {
                     opt.ClientId = config["Authentication:Google:ClientId"] ?? throw new Exception("Google ClientId is missing");
                     opt.ClientSecret = config["Authentication:Google:ClientSecret"] ?? throw new Exception("Google ClientSecret is missing");
                     opt.SaveTokens = true;
                     opt.CallbackPath = "/signin-google";

                     //opt.Events = new OAuthEvents
                     //{
                     //    OnRedirectToAuthorizationEndpoint = context =>
                     //    {
                     //        context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
                     //        return Task.CompletedTask;
                     //    }
                     //};
                 });
            services.AddScoped<IExternalLoginService, ExternalLoginService>();
            return services;
        
        }
    }
}

