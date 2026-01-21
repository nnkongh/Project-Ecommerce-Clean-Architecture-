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
using Ecommerce.Web.Dependencies;
using Ecommerce.Web.Interface;
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

            //Shared Layers
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(UserProfile));
            //MVC-Specific
            builder.Services.AddWebMvcServices(builder.Configuration);
            builder.Services.AddControllersWithViews();

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

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
