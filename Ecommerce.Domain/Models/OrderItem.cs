using Ecommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; } 
        public int Quantity { get; set; }


        public void IncreasingQuantity(int quantity) => Quantity += quantity;
        public void DecreasingQuantity(int quantity)
        {
            if(quantity >= 0)
            {
                Quantity -= quantity;
            }
            else
            {
                Quantity = 0;
            }
        }
        public static OrderItem Create(string imageUrl, string productName, int productId, decimal price, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Số lượng phải lớn hơn 0");
            if (price <= 0)
                throw new DomainException("Giá phải lớn hơn 0");
            if (string.IsNullOrEmpty(productName))
                throw new DomainException("Tên sản phẩm không được trống");

            return new OrderItem
            {
                ImageUrl = imageUrl,
                ProductName = productName,
                ProductId = productId,
                Quantity = quantity,
                Price = price,
            };
        }
    }
}
