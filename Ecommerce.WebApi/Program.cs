using Ecommerce.Application.Dependency;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.WebApi.DependencyInjection;
using Ecommerce.WebApi.Interfaces;
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
            builder.Services.AddAdapterServices();



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
