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
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending; 

        public static Order CreateOrder(string userId, string customerName, Address address)
        {
            var order = new Order()
            {
                CustomerId = userId,
                OrderStatus = OrderStatus.Pending,
                Address = address,
                CustomerName = customerName,
                OrderDate = DateTime.Now,
            };
            return order;
        }

        public void AddItem(int productId, int quantity, decimal unitPrice, string productName)
        {
            if(quantity <= 0)
            {
                throw new ArgumentException("Quantity cannot be negative");
            }
            if(unitPrice <= 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }
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

        private void CalculateTotal()
        {
            TotalAmount = Items.Sum(x => x.Quantity * x.Price);
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

    }
}
