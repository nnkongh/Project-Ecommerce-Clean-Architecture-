using Ecommerce.Application.Common.Command.AuthenticationExternal;
using Ecommerce.Application.Dependency;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Mappers;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.Infrastructure.Services.ExternalAuth;
using Ecommerce.WebApi.Dependencies;
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

            //Shared layer
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructure(builder.Configuration);

            //API-specific
            builder.Services.AddWebApiServices(builder.Configuration);
            builder.Services.AddControllers();

            //Background services            
            builder.Services.AddHostedService<RoleSeederHostedService>();

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
