using Ecommerce.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; } //
        public string CustomerName { get; set; } = null!;
        public Address? Address { get; set; }
        public User? User { get; set; }
        public List<OrderItem> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending; // Default status is Pending
        public void AddItem(int productId, int quantity, decimal unitPrice, string productName)
        {
            var items = Items.FirstOrDefault(x => x.ProductId == productId);
            if(items != null)
            {
                items.IncreasingQuantity(quantity);
            }
            else
            {
                var orderItem = new OrderItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = unitPrice,
                    ProductName = productName
                };
                Items.Add(orderItem);
            }
            CalculateTotal();
        }
        public void RemoveItem(int productId)
        {
            var items = Items.FirstOrDefault(x => x.ProductId == productId);
            if(items != null)
            {
                Items.Remove(items);
                CalculateTotal();
            }
        }
        private void CalculateTotal()
        {
            TotalAmount = Items.Sum(x => x.Price * x.Quantity);
        }
    }
}
