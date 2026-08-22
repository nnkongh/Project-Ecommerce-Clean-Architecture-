using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Infrastructure.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class CartCleanupBackgroundServiceTests : IDisposable
    {
        private readonly IServiceProvider _service;
        private readonly ApplicationDbContext _context;

        public CartCleanupBackgroundServiceTests()
        {
            var service = new ServiceCollection();
            service.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
            service.AddScoped<ICartRepository, CartRepository>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<ICartExpirationService, CartExpirationService>();
            _service = service.BuildServiceProvider();
            _context = _service.GetRequiredService<ApplicationDbContext>();
        }

        [Fact]
        public async Task HandleExpiredCartAsync_ShouldDeleteOnlyExpiredCarts()
        {
            // Arrange
            var expiredCart = Cart.CreateCart("user1");
            var activeCart = Cart.CreateCart("user2");

            _context.Carts.AddRange(expiredCart, activeCart);
            await _context.SaveChangesAsync();

            // CreateCart mặc định set ExpiredAt = Now (bị service coi là hết hạn),
            // nên chủ động set ExpiredAt cho 2 cart qua EF property entry (private setter)
            _context.Entry(expiredCart).Property(c => c.ExpiredAt).CurrentValue = DateTime.Now.AddDays(-1);
            _context.Entry(activeCart).Property(c => c.ExpiredAt).CurrentValue = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();

            var cartExpirationService = _service.GetRequiredService<ICartExpirationService>();

            // Act
            await cartExpirationService.HandleExpiredCartAsync(CancellationToken.None);

            // Assert
            var remainingCarts = await _context.Carts.ToListAsync();
            Assert.Single(remainingCarts);
            Assert.Equal(activeCart.Id, remainingCarts[0].Id);
        }

        [Fact]
        public async Task HandleExpiredCartAsync_ShouldNotDeleteActiveCart_WhenExpiredAtInFuture()
        {
            // Arrange
            var activeCart = Cart.CreateCart("user1");
            activeCart.AddItem(1, "product1", 2, 30);

            _context.Carts.Add(activeCart);
            await _context.SaveChangesAsync();

            var cartExpirationService = _service.GetRequiredService<ICartExpirationService>();

            // Act
            await cartExpirationService.HandleExpiredCartAsync(CancellationToken.None);

            // Assert
            var remainingCarts = await _context.Carts.ToListAsync();
            Assert.Single(remainingCarts);
            Assert.Equal(CartStatus.Active, remainingCarts[0].Status);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
