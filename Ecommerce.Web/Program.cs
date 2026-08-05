using Ecommerce.Application.Dependency;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.Infrastructure.Mapper;
using Ecommerce.Web.Dependencies;
using Ecommerce.Web.Exceptions;

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
                app.UseHsts();
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature?.Error is UnauthorizedException)
                    {
                        context.Response.Redirect("/Auth/Login?expired=true");
                        return;
                    }

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
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
