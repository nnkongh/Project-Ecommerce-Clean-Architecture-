namespace Ecommerce.Domain.Models
{
    public class Review
    {
        public int Id { get; private set; }

        public int Rating { get; private set; }

        public string Content { get; private set; } = string.Empty;

        public string UserId { get; private set; } = string.Empty;
        public int ProductId { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public User User { get; private set; } = null!;
        public Product Product { get; private set; } = null!;

        public Review() { }
        public static Review Create(
        string userId,
        int productId,
        int rating,
        string content)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating));

            return new Review
            {
                UserId = userId,
                ProductId = productId,
                Rating = rating,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void UpdateReview(int rating, string content)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating));

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Nội dung đánh giá không được để trống", nameof(content));

            Rating = rating;
            Content = content;
        }
    }
}
