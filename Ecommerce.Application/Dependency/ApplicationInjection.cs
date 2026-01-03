using AutoMapper;
using Ecommerce.Application.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Dependency
{
    public static class ApplicationInjection
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

            return services;



            // Checklist
            // Kiểm tra các dependency và đổi tên method nếu cần thiết
            // 


            // AddRepositoryServices
            // AddMediatRServices
            // AddExternalServices
            // AddObjectMapping
            //

        }
      
    }

}
