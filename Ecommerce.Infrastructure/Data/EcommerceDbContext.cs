using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class EcommerceDbContext : IdentityDbContext<AppUser>
    {
        public EcommerceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Wishlist> Wishlist { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Nếu không có dòng này thì migration sẽ tạo lại các bảng đã có

            modelBuilder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId);

            modelBuilder.Entity<Order>()
                .OwnsOne(x => x.Address); 

            modelBuilder.Entity<User>()
                .OwnsOne(x => x.Address);

            modelBuilder.Entity<Wishlist>()
                .HasMany(x => x.Items)
                .WithOne(x => x.WishList)
                .HasForeignKey(x => x.WishListId)
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
