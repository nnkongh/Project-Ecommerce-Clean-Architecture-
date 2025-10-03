using Ecommerce.Infrastructure.Dependency_Injection;
using Ecommerce.WebApi.DependencyInjection;
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
            builder.Services.AddControllers();
            builder.Services.AddInfrastructureServices(builder.Configuration);
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
