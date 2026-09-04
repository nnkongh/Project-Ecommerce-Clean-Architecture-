using AutoMapper;
using Ecommerce.Application.Common.Command.Reviews.CreateReview;
using Ecommerce.Application.Common.Command.Reviews.DeleteReview;
using Ecommerce.Application.Common.Command.Reviews.UpdateReview;
using Ecommerce.Application.DTOs.Models;
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
    public class ReviewHandlerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IReviewRepository _reviewRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ReviewHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _reviewRepo = new ReviewRepository(_context);
            _productRepo = new ProductRepository(_context);
            _uow = new UnitOfWork(_context);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<Ecommerce.Application.Mappers.ObjectMapper>())
                      .CreateMapper();
        }

        private async Task<Product> SeedProductAsync()
        {
            var product = Product.Create("Sản phẩm test", "img.jpg", shopId: 1, price: 100, stock: 10,ParentCategoryId: 1, ChildCategoryId: 3);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        private async Task<User> SeedUserAsync(string userId = "user1")
        {
            var user = User.Create("tester", "tester@test.com", "0123456789");
            // User.Id có private setter nên set qua EF property entry trước khi Add
            _context.Entry(user).Property(u => u.Id).CurrentValue = userId;
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task<Review> SeedReviewAsync(string userId = "user1", int rating = 4, string content = "Hài lòng")
        {
            await SeedUserAsync(userId);
            var product = await SeedProductAsync();
            var review = Review.Create(userId, product.Id, rating, content);
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        // ---------- CreateReviewCommandHandler ----------

        [Fact]
        public async Task CreateReview_ShouldSaveAndReturnModel_WhenValid()
        {
            await SeedUserAsync("user1");
            var product = await SeedProductAsync();
            var handler = new CreateReviewCommandHandler(_productRepo, _reviewRepo, _uow, _mapper);
            var command = new CreateReviewCommand(Rating: 5, Content: "Rất tốt", ProductId: product.Id, UserId: "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(5, result.Value.Rating);
            Assert.Equal("Rất tốt", result.Value.Content);
            Assert.Equal(product.Id, result.Value.ProductId);
            Assert.Equal("user1", result.Value.UserId);

            var saved = await _context.Reviews.SingleAsync(r => r.ProductId == product.Id);
            Assert.Equal(5, saved.Rating);
        }

        [Fact]
        public async Task CreateReview_ShouldReturnFailure_WhenProductNotFound()
        {
            var handler = new CreateReviewCommandHandler(_productRepo, _reviewRepo, _uow, _mapper);
            var command = new CreateReviewCommand(5, "Rất tốt", ProductId: 9999, UserId: "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Empty(await _context.Reviews.ToListAsync());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public async Task CreateReview_ShouldReturnFailure_WhenRatingOutOfRange(int rating)
        {
            var product = await SeedProductAsync();
            var handler = new CreateReviewCommandHandler(_productRepo, _reviewRepo, _uow, _mapper);
            var command = new CreateReviewCommand(rating, "Nội dung", product.Id, "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateReview_ShouldReturnFailure_WhenContentIsInvalid(string? content)
        {
            var product = await SeedProductAsync();
            var handler = new CreateReviewCommandHandler(_productRepo, _reviewRepo, _uow, _mapper);
            var command = new CreateReviewCommand(4, content!, product.Id, "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        // ---------- UpdateReviewCommandHandler ----------

        [Fact]
        public async Task UpdateReview_ShouldUpdateRatingAndContent_WhenOwnerUpdates()
        {
            var review = await SeedReviewAsync(userId: "user1");
            var handler = new UpdateReviewCommandHandler(_reviewRepo, _uow, _mapper);
            var command = new UpdateReviewCommand(review.Id, Rating: 2, Content: "Đổi ý rồi", UserId: "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Rating);
            Assert.Equal("Đổi ý rồi", result.Value.Content);

            var saved = await _context.Reviews.SingleAsync(r => r.Id == review.Id);
            Assert.Equal(2, saved.Rating);
        }

        [Fact]
        public async Task UpdateReview_ShouldReturnFailure_WhenReviewNotFound()
        {
            var handler = new UpdateReviewCommandHandler(_reviewRepo, _uow, _mapper);
            var command = new UpdateReviewCommand(9999, 5, "Nội dung", "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateReview_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            var review = await SeedReviewAsync(userId: "user1");
            var handler = new UpdateReviewCommandHandler(_reviewRepo, _uow, _mapper);
            var command = new UpdateReviewCommand(review.Id, 5, "Hack nội dung", UserId: "user2");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            var saved = await _context.Reviews.SingleAsync(r => r.Id == review.Id);
            Assert.Equal(4, saved.Rating); // không bị thay đổi
        }

        [Fact]
        public async Task UpdateReview_ShouldReturnFailure_WhenRatingOutOfRange()
        {
            var review = await SeedReviewAsync();
            var handler = new UpdateReviewCommandHandler(_reviewRepo, _uow, _mapper);
            var command = new UpdateReviewCommand(review.Id, 10, "Nội dung", "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        // ---------- DeleteReviewCommandHandler ----------

        [Fact]
        public async Task DeleteReview_ShouldDeleteReview_WhenOwnerDeletes()
        {
            var review = await SeedReviewAsync(userId: "user1");
            var handler = new DeleteReviewCommandHandler(_reviewRepo, _uow);
            var command = new DeleteReviewCommand(review.Id, "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Empty(await _context.Reviews.ToListAsync());
        }

        [Fact]
        public async Task DeleteReview_ShouldReturnFailure_WhenReviewNotFound()
        {
            var handler = new DeleteReviewCommandHandler(_reviewRepo, _uow);
            var command = new DeleteReviewCommand(9999, "user1");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteReview_ShouldReturnFailure_WhenUserIsNotOwner()
        {
            var review = await SeedReviewAsync(userId: "user1");
            var handler = new DeleteReviewCommandHandler(_reviewRepo, _uow);
            var command = new DeleteReviewCommand(review.Id, "user2");

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Single(await _context.Reviews.ToListAsync()); // vẫn còn
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
