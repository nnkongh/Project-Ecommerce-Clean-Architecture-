using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Application.Interfaces.Core;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Infrastructure.Authen;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Mapper;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Infrastructure.Repository.Base;
using Ecommerce.Infrastructure.Repository.UnitOfWork;
using Ecommerce.Infrastructure.Repository.User_Repository;
using Ecommerce.Infrastructure.Services.Authen;
using Ecommerce.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Dependency_Injection
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Add DbContext
            services.AddDbContext<EcommerceDbContext>(opt =>
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                opt.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(ServiceContainer).Assembly.FullName));
            });

            // Add Authentication
            var jwtSettings = config.GetSection("Jwt");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"] ?? throw new NullReferenceException("Jwt Issuer is missing"),
                        ValidAudience = jwtSettings["Audience"] ?? throw new NullReferenceException("Jwt Audience is missing"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new NullReferenceException("Jwt Key is missing")))
                    };

                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Request.Cookies.TryGetValue("access_token", out var accessToken);
                            if(!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // AddIdentiy
            services.AddIdentityCore<AppUser>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequireUppercase = false;
                x.Password.RequiredLength = 6;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireLowercase = true;
            })
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<EcommerceDbContext>();

            /// Figure Identity Token Lifetime
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(2);
            });


            // Add MediaTr
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Ecommerce.Application.AssemblyReference).Assembly));

            // Add AutoMapper
            services.AddAutoMapper(typeof(ObjectMapper).Assembly);
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            // Add Seperate Authentication Service
            services.AddScoped<IUserManagementService, UserManagementRepository>();
            services.AddScoped<IUserAuthenticationService,UserAuthenticationRepository>();
            services.AddScoped<IUserTokenService,UserTokenRepository>();
            services.AddScoped<IUserRoleService,UserRoleRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();


            // Add Generic Service
            services.AddScoped(typeof(IRepositoryBase<>), typeof(GenericRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository,CartRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var email = config.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(email);
            services.AddScoped<IEmailService, EmailService>();


            // Add Role Service from HostedService
            services.AddHostedService<RoleSeederHostedService>();

            return services;

            
        }

    }
}
