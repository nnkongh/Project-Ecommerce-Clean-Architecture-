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
        public List<CartItem> Items { get; set; } = [];


        public static Cart CreateCart(string UserId)
        {
            var cart = new Cart()
            {
                UserId = UserId,
            };
            return cart;
        }
        public void AddItem(int productId, int quantity, decimal unitPrice, string productName)
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
                    Quantity = quantity,
                    ProductId = productId,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                });
            }
        }
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if(item != null)
            {
                Items.Remove(item);
                item.DecreaseQuantity(item.Quantity);
            }
        }
        public void ReduceItemQuantity(int productId,int quantity) {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if(item != null)
            {
                item.DecreaseQuantity(quantity);
            }
        }
        public void Clear()
        {
            Items.Clear();
        }
    }
}
