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

        public void AddItem(int productId, int quantity)
        {
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
    }
}
