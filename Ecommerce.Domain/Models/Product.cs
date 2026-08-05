using Ecommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = [];

        public Product() { }
        public static Product Create(string name, string imageUrl, decimal price, int stock, int categoryId, string? description = null)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Tên sản phẩm không được để trống");
            if (price < 0) throw new DomainException("Giá sản phẩm không được để trống");
            if (stock < 0) throw new DomainException("Số lượng hàng không được để trống");
            if (string.IsNullOrWhiteSpace(imageUrl)) throw new DomainException("Hình ảnh sản phẩm không được để trống");

            var product = new Product()
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                Price = price,
                Stock = stock,
                CategoryId = categoryId,
                IsActive = true
            };
            return product;
        }
        public void UpdateDetail(string name, string? description = null)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Tên sản phẩm không được để trống");
            Name = name;
            Description = description;
        }
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0) throw new DomainException("Giá sản phẩm không được để trống");
            Price = newPrice;
        }
        public void AdjustStock(int stock)
        {
            Stock += stock;
        }
    }
}
