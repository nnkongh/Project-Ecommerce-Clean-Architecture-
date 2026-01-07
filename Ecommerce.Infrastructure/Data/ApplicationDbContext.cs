using Ecommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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
                .Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CartItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .OwnsOne(x => x.Address, a =>
                {
                    a.Property(p => p.City).IsRequired();
                    
                });


            modelBuilder.Entity<User>()
                .OwnsOne(x => x.Address, a =>
                {
                    a.Property(p => p.City).IsRequired();
                });

            modelBuilder.Entity<Wishlist>()
                .HasMany(x => x.Items)
                .WithOne(x => x.WishList)
                .HasForeignKey(x => x.WishListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(x => x.User)
                .WithMany(x => x.Wishlist)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
