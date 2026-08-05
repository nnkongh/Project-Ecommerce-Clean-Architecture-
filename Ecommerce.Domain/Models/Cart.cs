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
        public int Id { get; private set; }
        public User? User { get; private set; }
        public string? UserId { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? ExpiredAt { get; private set; }
        public CartStatus Status { get; private set; }
        public ICollection<CartItem> Items => _items.AsReadOnly();
        private readonly List<CartItem> _items = new List<CartItem>();

        private Cart() { }
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
        public void AddItem(int productId, string productName, int quantity, decimal unitPrice, string? imageUrl = null)
        {
            var item = GetItem(productId);
            if (item != null)
            {
                item.IncreaseQuantity();
            }
            else
            {
                var cartItem = CartItem.Create(productId, productName, quantity, unitPrice, imageUrl);
                _items.Add(cartItem);
                MarkAsUpdated();
            }
        }
        public void RemoveItem(CartItem item)
        {
            _items.Remove(item);
            MarkAsUpdated();
        }
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = GetItem(productId);
            if (item != null)
            {
                item.SetQuantity(quantity);
                MarkAsUpdated();
            }
        }
        public void Clear()
        {
            _items.Clear();
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
        public CartItem? GetItem(int productId)
        {
            return Items.SingleOrDefault(x => x.ProductId == productId);
        }
    }
}
