using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class SeedRole
    {
        public static async Task SeedRoleAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppIdentityDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<SeedRole>>();
            try
            {
                await context.Database.EnsureCreatedAsync();
                await AddRolesAsync(roleManager, "Admin");
                await AddRolesAsync(roleManager, "User");

                string adminuser = "admin";
                var admin = await userManager.FindByNameAsync(adminuser);
                if(admin == null)
                {
                    admin = new AppUser
                    {
                        UserName = "admin",
                        Email = "",
                        EmailConfirmed = true,
                        NormalizedEmail = "",
                        NormalizedUserName = "",
                    };
                    var result = await userManager.CreateAsync(admin, "Admin123@");
                    if (!result.Succeeded)
                    {
                        logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding roles: {ex.Message}");
            }
        }

        private static async Task AddRolesAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
