using Ecommerce.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public User? User { get; set; }        
        public string? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ExpiredAt { get;set; }
        public CartStatus Status { get; set; }
        public List<CartItem> Items { get; set; } = [];


        public static Cart CreateCart(string UserId)
        {
            var cart = new Cart()
            {
                UserId = UserId,
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = CartStatus.Active
            };
            return cart;
        }
        public void AddItem(int productId, int quantity, string? ImageUrl, decimal unitPrice, string productName)
        {
            if(quantity < 0 || unitPrice < 0)
            {
                throw new ArgumentException("Quantity/Price cannot be negative");
            }
            var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
            if(existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                Items.Add(new CartItem()
                {
                    ImageUrl = ImageUrl,
                    Quantity = quantity,
                    ProductId = productId,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                });
            }
            MarkAsUpdated();
        }
        public void AddItemFromWishlist(int productId, string productName, decimal unitPrice, int quantity = 1)  // 1 Wishlist không chứa nhiều product có cùng 1 id
        {
            var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
               
            }
            else
            {
                Items.Add(new CartItem()
                {
                    ProductId = productId,
                    ProductName = productName,
                    Quantity = quantity,
                    UnitPrice = unitPrice,

                });
            }
            MarkAsUpdated();
        }
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if(item != null)
            {
                Items.Remove(item);
                item.DecreaseQuantity(item.Quantity);
            }
            MarkAsUpdated();
        }
        public void ReduceItemQuantity(int productId,int quantity) {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if(item != null)
            {
                item.DecreaseQuantity(quantity);
            }
            MarkAsUpdated();
        }
        public void Clear()
        {
            Items.Clear();
        }
        public void MarkAsExpired()
        {
            Status = CartStatus.Expired;
            UpdatedAt = DateTime.UtcNow;
        }
        private void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
            ExpiredAt = DateTime.UtcNow.AddDays(1);
        }
    }
}
