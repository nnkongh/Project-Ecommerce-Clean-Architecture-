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
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string CustomerName { get; set; } = null!;
        public Address? Address { get; set; }
        public User? User { get; set; }
        public List<OrderItem> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending; 

        public static Order CreateOrder(string userId, string customerName, string phoneNumber, string email, string city, string ward, string district, string street, string? province)
        {
            var order = new Order()
            {
                CustomerId = userId,
                PhoneNumber = phoneNumber,
                Email = email,
                OrderStatus = OrderStatus.Pending,
                Address = Address.Create(district, city, province, street, ward),
                CustomerName = customerName,
                OrderDate = DateTime.Now,
            };
            return order;
        }

        public void AddItem(int productId, int quantity, string? ImageUrl, decimal unitPrice, string productName)
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
                    ImageUrl = ImageUrl,
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
