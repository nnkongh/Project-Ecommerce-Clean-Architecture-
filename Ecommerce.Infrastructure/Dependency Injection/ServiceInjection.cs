using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.Infrastructure.Mapper;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Infrastructure.Repository.Base;
using Ecommerce.Infrastructure.Repository.UnitOfWork;
using Ecommerce.Infrastructure.Services.Authen;
using Ecommerce.Infrastructure.Services.Authentication;
using Ecommerce.Infrastructure.Services.Email;
using Ecommerce.Infrastructure.Services.ExternalAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
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
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
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
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddGoogle(opt =>
                {
                    opt.ClientId = config["Authentication_Google_ClientId"]!;
                    opt.ClientSecret = config["Authentication_Google_ClientSecret"]!;
                    opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.SaveTokens = true;

                    opt.Events = new OAuthEvents
                    {
                        OnCreatingTicket = context =>
                        {
                            var accessToken = context.AccessToken;

                            var idToken = context.Properties.GetTokenValue("id_token");

                            var handler = new JwtSecurityTokenHandler();
                            var jwt = handler.ReadJwtToken(idToken);

                            var sub = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                            var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

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
            services.AddScoped<IIdentityManagementUserProvider, UserManagementService>();
            services.AddScoped<IIdentityUserProvider,UserAuthenticationService>();
            services.AddScoped<IUserTokenService,UserTokenService>();
            services.AddScoped<IIdentityRole,UserRoleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();


            // Add Generic Service
            services.AddScoped(typeof(IRepositoryBase<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository,CartRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IWishlistRepository,WishlistRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add external provider
            services.AddScoped<IExternalAuthProvider, GoogleAuthProvider>();
            var email = config.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(email);
            services.AddScoped<IEmailService, EmailService>();


            // Add Role Service from HostedService
            services.AddHostedService<RoleSeederHostedService>();

            return services;

            
        }

    }
}
