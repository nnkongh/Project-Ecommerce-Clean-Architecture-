using Ecommerce.Application.Common.Command.AuthenticationExternal;
using Ecommerce.Application.Dependency;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.Infrastructure.Services.ExternalAuth;
using Ecommerce.WebApi.DependencyInjection;
using Ecommerce.WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Ecommerce.WebApi
{
    public partial class Program
    {
        public static void Main(string[] args)
        {   
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration.AddUserSecrets<Program>(optional: true).AddEnvironmentVariables();

            //builder.Services.AddApplicationServices();
            builder.Services.AddControllers();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddMediatRServices();
            builder.Services.AddAdapterServices();
            builder.Services.AddScoped<IExternalLoginService, ExternalLoginService>();
            builder.Services.AddScoped<ICookieTokenService, CookieTokenService>();

            var app = builder.Build();

         
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
