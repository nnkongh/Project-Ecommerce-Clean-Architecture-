using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Shared;
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
        public decimal TotalAmount { get; set; }
        public Address? Address { get; set; }
        public User? User { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public ICollection<OrderItem> Items => _items.AsReadOnly();
        private readonly List<OrderItem> _items = new List<OrderItem>();

        private Order() { }
        public static Order CreateOrder(string customerId, string customerName, string? phoneNumber, string? email, Address address)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new DomainException("Số điện thoại không được để trống");
            if (string.IsNullOrEmpty(email)) throw new DomainException("Email không không được để trống");
            if (string.IsNullOrWhiteSpace(customerName)) throw new DomainException("Tên khách hàng không được để trống");
            if (address == null) throw new DomainException("Địa chỉ không được để trống");

            var order = new Order
            {
                CustomerId = customerId,
                PhoneNumber = phoneNumber,
                Email = email,
                OrderStatus = OrderStatus.Pending,
                Address = address,
                CustomerName = customerName,
                OrderDate = DateTime.Now,
            };
            return order;
        }

        public void AddItem(string imageUrl, string productName, int productId, decimal price, int quantity)
        {
            if (IsExistItem(productId))
            {
                var item = FindOrderItem(productId);
                item!.Quantity++;
            }
            var orderItem = OrderItem.Create(imageUrl, productName, productId, price, quantity);
            _items.Add(orderItem);
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            TotalAmount = Items.Sum(x => x.Quantity * x.Price);
        }

        public void RemoveItem(OrderItem item)
        {
            Items.Remove(item);
            CalculateTotal();
        }
        public OrderItem? FindOrderItem(int productId)
        {
            return Items.FirstOrDefault(x => x.ProductId == productId);
        }
        private bool IsExistItem(int productId)
        {
            return Items.Any(x => x.ProductId == productId);
        }

    }
}
