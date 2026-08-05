using Ecommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class CartItem
    {
        public int Id { get; private set; }
        public string ProductName { get; private set; } = default!;
        public int Quantity { get; private set; }
        public decimal TotalPrice => Quantity * UnitPrice;
        public string? ImageUrl { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public Cart Cart { get; private set; } = default!;


        public static CartItem Create(int productId, string productName, int quantity, decimal unitprice, string? imageUrl = null)
        {
            if (string.IsNullOrEmpty(productName)) throw new DomainException("Tên sản phẩm không được để trống");
            if (quantity < 0) throw new DomainException("Số lượng sản phẩm không được bé hơn 0");
            if (unitprice < 0) throw new DomainException("Giá không được bé hơn 0");

            return new CartItem
            {
                ProductId = productId,
                ImageUrl = imageUrl,
                ProductName = productName,
                Quantity = quantity,
                UnitPrice = unitprice,
            };
        }
        public decimal TotalPriceItems() => TotalPrice;
        public void SetQuantity(int quantity) => Quantity = quantity;
        public void IncreaseQuantity() => Quantity++;
    }
}
