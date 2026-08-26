using Ecommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; } = null!;
        public bool IsActive { get; private set; }
        public string ImageUrl { get; private set; } = null!;
        public decimal Price { get; private set; }
        public int? ShopId { get; private set; }
        public Shop? Shop { get; private set; }
        public int Stock { get; private set; }
        public int ParentCategoryId { get; private set; }
        public int ChildCategoryId { get; private set; }
        public Category Category { get; private set; } = null!;
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
        private readonly List<Review> _reviews = new List<Review>();
        private readonly List<Comment> _comments = new List<Comment>();

        public Product() { }
        public static Product Create(string name, string imageUrl, int shopId, decimal price, int stock, int ParentCategoryId, int ChildCategoryId, string? description = null)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Tên sản phẩm không được để trống");
            if (price < 0) throw new DomainException("Giá sản phẩm không được để trống");
            if (stock < 0) throw new DomainException("Số lượng hàng không được để trống");
            if (string.IsNullOrWhiteSpace(imageUrl)) throw new DomainException("Hình ảnh sản phẩm không được để trống");

            var product = new Product()
            {
                Name = name,
                Description = description,
                ShopId = shopId,
                ImageUrl = imageUrl,
                Price = price,
                Stock = stock,
                ParentCategoryId = ParentCategoryId,
                ChildCategoryId = ChildCategoryId,
                IsActive = true
            };
            return product;
        }
        public void AdjustStock(int stock)
        {
            Stock += stock;
        }
        public void UpdateProduct(string? name = null, string? description = null, string? imageUrl = null, decimal? price = null, int? stock = null)
        {
            if (price.HasValue && price.Value < 0) throw new DomainException("Giá sản phẩm không được âm");
            if (stock.HasValue && stock.Value < 0) throw new DomainException("Tồn kho không được âm");

            Name = name ?? Name;
            Description = description ?? Description;
            ImageUrl = imageUrl ?? ImageUrl;
            Price = price ?? Price;
            Stock = stock ?? Stock;

        }
    }
}
