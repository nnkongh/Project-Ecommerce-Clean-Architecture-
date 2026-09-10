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
        public ICollection<SubOrder> SubOrders => _subOrders.AsReadOnly();
        private readonly List<SubOrder> _subOrders = new List<SubOrder>();

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
        public void UpdateStatus(OrderStatus orderStatus)
        {
            OrderStatus = orderStatus;
        }
        public void AddSubOrder(SubOrder subOrder)
        {
            if (subOrder == null) throw new DomainException("SubOrder không được để trống");
            if (IsExistSubOrder(subOrder.ShopId))
            {
                var existing = FindSubOrder(subOrder.ShopId);
                foreach (var item in subOrder.Items)
                {
                    if (item != null)
                    {
                        existing!.AddItem(item.ImageUrl!, item.ProductName!, item.ProductId, item.Price, item.Quantity);
                    }
                }
                CalculateTotal();
                return;
            }
            _subOrders.Add(subOrder);
            CalculateTotal();
        }
        public SubOrder? FindSubOrder(int shopId)
        {
            return SubOrders.FirstOrDefault(x => x.ShopId == shopId);
        }
        public void RemoveSubOrder(SubOrder subOrder)
        {
            _subOrders.Remove(subOrder);
            CalculateTotal();
        }
        private bool IsExistSubOrder(int shopId)
        {
            return SubOrders.Any(x => x.ShopId == shopId);
        }
        private void CalculateTotal()
        {
            TotalAmount = SubOrders.Sum(x => x.TotalAmount);
        }
    }
}