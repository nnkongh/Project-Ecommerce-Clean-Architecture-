using Ecommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Comment
    {
        public int Id { get; private set; }
        public string Content { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; }
        public int ProductId { get; private set; }      
        public string UserId { get; private set; } = default!;
        public User User { get; private set; } = default!;
        public Product Product { get; private set; } = default!;

        private Comment() { }

        public static Comment Create(string content, int productId, string userId)
        {
            if (string.IsNullOrEmpty(content)) throw new DomainException("Nội dung không được để trống");

            return new Comment
            {
                Content = content,
                ProductId = productId,
                UserId = userId,
                CreatedAt = DateTime.Now,
            };
        }
        public void UpdateComment(string content)
        {
            if (string.IsNullOrEmpty(content)) throw new DomainException("Nội dung không được để trống");

            Content = content;
        }
    }
}
