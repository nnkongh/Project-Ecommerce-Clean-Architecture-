using AutoMapper;
using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Infrastructure.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class ShopHandlerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IShopRepository _shopRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ShopHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _shopRepo = new ShopRepository(_context);
            _uow = new UnitOfWork(_context);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<Ecommerce.Application.Mappers.ObjectMapper>())
                      .CreateMapper();
        }

        private async Task<Shop> SeedShopAsync(string userId = "user1", string name = "Cửa hàng của user1")
        {
            var shop = Shop.Create(name, userId);
            await _context.Shops.AddAsync(shop);
            await _context.SaveChangesAsync();
            return shop;
        }

        // ---------- CreateShopHandler ----------

        [Fact]
        public async Task CreateShop_ShouldSaveAndReturnModel_WhenValid()
        {
            var handler = new CreateShopHandler(_shopRepo, _uow, _mapper);
            var command = new CreateShopCommand("user1", "Shop mới");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("user1", result.Value.UserId);
            Assert.True(result.Value.IsActive);

            var saved = await _context.Shops.SingleAsync();
            Assert.Equal("Shop mới", saved.Name);
            Assert.True(saved.IsActive);
        }

        [Fact]
        public async Task CreateShop_ShouldReturnFailure_WhenUserAlreadyOwnsAShop()
        {
            await SeedShopAsync(userId: "user1");
            var handler = new CreateShopHandler(_shopRepo, _uow, _mapper);
            var command = new CreateShopCommand("user1", "Shop thứ hai");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Single(await _context.Shops.ToListAsync());
        }

        // ---------- UpdateShopHandler ----------

        [Fact]
        public async Task UpdateShop_ShouldChangeName_WhenShopExists()
        {
            var shop = await SeedShopAsync();
            var handler = new UpdateShopHandler(_uow, _shopRepo, _mapper);
            var command = new UpdateShopCommand("user1", shop.Id, "Tên đã đổi");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            var saved = await _context.Shops.SingleAsync(s => s.Id == shop.Id);
            Assert.Equal("Tên đã đổi", saved.Name);
        }

        [Fact]
        public async Task UpdateShop_ShouldReturnFailure_WhenShopNotFound()
        {
            var handler = new UpdateShopHandler(_uow, _shopRepo, _mapper);
            var command = new UpdateShopCommand("user1", 9999, "Tên mới");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        // ---------- DeleteShopHandler ----------

        [Fact]
        public async Task DeleteShop_ShouldDeleteShop_WhenOwnerDeletes()
        {
            var shop = await SeedShopAsync(userId: "user1");
            var handler = new DeleteShopHandler(_uow, _shopRepo);
            var command = new DeleteShopCommand("user1", shop.Id);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Empty(await _context.Shops.ToListAsync());
        }

        [Fact]
        public async Task DeleteShop_ShouldReturnFailure_WhenShopNotFound()
        {
            var handler = new DeleteShopHandler(_uow, _shopRepo);
            var command = new DeleteShopCommand("user1", 9999);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteShop_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            var shop = await SeedShopAsync(userId: "user1");
            var handler = new DeleteShopHandler(_uow, _shopRepo);
            var command = new DeleteShopCommand("user2", shop.Id);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Single(await _context.Shops.ToListAsync());
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
