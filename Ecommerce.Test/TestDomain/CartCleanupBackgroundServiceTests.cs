using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Infrastructure.Repository.Base;
using Ecommerce.Infrastructure.Repository.UnitOfWork;
using Ecommerce.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class CartCleanupBackgroundServiceTests
    {
        private readonly IServiceProvider _service;
        private readonly ApplicationDbContext _context;
        public CartCleanupBackgroundServiceTests()
        {
            var service = new ServiceCollection();
            service.AddDbContext<AppIdentityDbContext>(opt =>
            {
                opt.UseInMemoryDatabase("hi");
            });
            //service.AddScoped(typeof(IRepositoryBase<,>), typeof(GenericRepository<,>));
            //service.AddScoped<IUserRepository, UserRepository>();
            //service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<ICartRepository, CartRepository>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            //service.AddScoped<IOrderRepository, OrderRepository>();
            //service.AddScoped<IWishlistRepository, WishlistRepository>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            service.AddScoped<ICartExpirationService, CartExpirationService>();
            _service = service.BuildServiceProvider();
            _context = _service.GetRequiredService<ApplicationDbContext>();
        }
        [Fact]
        public async Task DeleteCart_WhenCartExpired()
        {
            var expiredCart = new Cart
            {
                UserId = "abc",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-10),
                ExpiredAt = DateTime.UtcNow.AddDays(-1),  // 👈 thêm dòng này
                Status = CartStatus.Active
            };
            var activeCart = new Cart
            {
                UserId = "user2",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Carts.AddRange(expiredCart, activeCart);
            await _context.SaveChangesAsync();


            //using var scope = _service.CreateScope();
            //var cartService = scope.ServiceProvider.GetRequiredService<ICartExpirationService>();
            //await cartService.HandleExpiredCartAsync(CancellationToken.None);

            //var service = new CartBackgroundService(_service);

            using var cts = new CancellationTokenSource();

            cts.CancelAfter(TimeSpan.FromSeconds(5));

            //var task = service.StartAsync(cts.Token);
            await Task.Delay(2000);
            //await service.StopAsync(CancellationToken.None);

            var remainingCarts = await _context.Carts.ToListAsync();
            Assert.Single(remainingCarts);
            Assert.Equal(activeCart.Id, remainingCarts[0].Id); 
        }
    }
}
