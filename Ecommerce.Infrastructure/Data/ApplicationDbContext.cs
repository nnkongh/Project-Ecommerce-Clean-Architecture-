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
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<UserAddress> UserAddresses { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Shop> Shops { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;


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

            modelBuilder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ParentCategoryId);

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

            modelBuilder.Entity<UserAddress>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<UserAddress>()
                .Property(x => x.Street).IsRequired();
            modelBuilder.Entity<UserAddress>()
                .Property(x => x.Ward).IsRequired();
            modelBuilder.Entity<UserAddress>()
                .Property(x => x.District).IsRequired();
            modelBuilder.Entity<UserAddress>()
                .Property(x => x.City).IsRequired();

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

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Shop>()
                .HasMany(s => s.Products)
                .WithOne(s => s.Shop)
                .HasForeignKey(s => s.ShopId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(r => r.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Shop)
                .WithOne(u => u.User)
                .HasForeignKey<Shop>(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Review>()       
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Shop>()
                .OwnsOne(s => s.Address);

        }
    }
}
