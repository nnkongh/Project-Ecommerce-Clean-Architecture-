using AutoMapper.Internal.Mappers;
using Ecommerce.Application.Common.Command.Authentication.Login;
using Ecommerce.Application.Common.Command.AuthenticationExternal;
using Ecommerce.Application.Dependency;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.Infrastructure.Mapper;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Infrastructure.Repository.UnitOfWork;
using Ecommerce.Infrastructure.Services.Authentication;
using Ecommerce.Infrastructure.Services.Email;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Mapping;
using Ecommerce.Web.Services;
using Ecommerce.Web.ViewModels.ApiResponse;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace Ecommerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            //builder.Services.AddScoped<IPrincipalFactory, PrincipalFactory>();
            //builder.Services.AddScoped<SignInManager<>>
            builder.Services.AddConnectionDatabase(builder.Configuration);
            //builder.Services.AddIdentityService();

            builder.Services.AddIdentityService();

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ICookieTokenService, CookieTokenService>();
            builder.Services.AddConnectionDatabase(builder.Configuration);
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPrincipalFactory, PrincipalFactory>();
            builder.Services.AddRepositories();
            builder.Services.AddMediatRServices();

            //builder.Services.AddMediatRServices();

            builder.Services.AddAutoMapper(typeof(ObjectMapper));

            
            //AddIdentity tự động đăng ký các scheme như ApplicationScheme, ExternalScheme, TwoFactorRememberMeScheme
            // Khi sử dụng addsigninmanager, thì asp.net identity sẽ dùng các scheme mặc định khi gọi các method như
            // getexternallogininfoasync và externalloginsigninasync

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                   .AddDefaultTokenProviders()
                   .AddSignInManager()
                   .AddEntityFrameworkStores<AppIdentityDbContext>();



            builder.Services.AddExternalAuthentication(builder.Configuration);
            builder.Services.AddHttpClientService(builder.Configuration);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;

                    ApiResponse<string>.Fail("Failed");
                });
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
