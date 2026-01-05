using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.Infrastructure.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Dependency_Injection
{
    public static class EmailInjection
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailConfiguration>(config.GetSection("EmailConfiguration"));
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
