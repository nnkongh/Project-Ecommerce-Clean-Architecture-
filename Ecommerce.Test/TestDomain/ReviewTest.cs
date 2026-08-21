using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class ReviewTest
    {
        [Fact]
        public void Create_ShouldReturnReviewWithCorrectValues_WhenValidInput()
        {
            // Arrange
            var userId = "user1";
            var productId = 1;
            var rating = 5;
            var content = "Sản phẩm rất tốt";

            // Act
            var review = Review.Create(userId, productId, rating, content);

            // Assert
            Assert.Equal(userId, review.UserId);
            Assert.Equal(productId, review.ProductId);
            Assert.Equal(rating, review.Rating);
            Assert.Equal(content, review.Content);
            Assert.True(review.CreatedAt <= DateTime.UtcNow.AddSeconds(1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(100)]
        public void Create_ShouldThrowArgumentOutOfRangeException_WhenRatingOutOfRange(int rating)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Review.Create("user1", 1, rating, "Nội dung"));
        }

        [Fact]
        public void Create_ShouldAllowEmptyContent_WhenContentIsNotValidated()
        {
            // Act - hiện tại Review.Create không validate content
            var review = Review.Create("user1", 1, 4, "");

            // Assert
            Assert.Equal("", review.Content);
        }

        [Fact]
        public void UpdateReview_ShouldUpdateRatingAndContent_WhenValidInput()
        {
            var review = Review.Create("user1", 1, 3, "Bình thường");

            review.UpdateReview(5, "Sau thời gian sử dụng vẫn tốt");

            Assert.Equal(5, review.Rating);
            Assert.Equal("Sau thời gian sử dụng vẫn tốt", review.Content);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(6)]
        public void UpdateReview_ShouldThrowArgumentOutOfRangeException_WhenRatingOutOfRange(int rating)
        {
            var review = Review.Create("user1", 1, 3, "Bình thường");

            Assert.Throws<ArgumentOutOfRangeException>(() => review.UpdateReview(rating, "Nội dung mới"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateReview_ShouldThrowArgumentException_WhenContentIsInvalid(string? content)
        {
            var review = Review.Create("user1", 1, 3, "Bình thường");

            Assert.Throws<ArgumentException>(() => review.UpdateReview(4, content!));
        }

        [Fact]
        public void UpdateReview_ShouldNotChangeUserIdAndProductId()
        {
            var review = Review.Create("user1", 10, 3, "Bình thường");

            review.UpdateReview(4, "Đánh giá lại");

            Assert.Equal("user1", review.UserId);
            Assert.Equal(10, review.ProductId);
        }
    }
}
