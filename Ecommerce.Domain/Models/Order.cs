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
        public List<OrderItem> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending; // Default status is Pending
        public Order(int id, string customerId, string customerName)
        {
            Id = id;
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            CustomerName = customerName;
            Items = new List<OrderItem>();
            TotalAmount = Items.Sum(item => item.Price * item.Quantity);
        }
    }
}
