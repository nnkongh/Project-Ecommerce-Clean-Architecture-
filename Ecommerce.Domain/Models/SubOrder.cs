using Ecommerce.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Domain.Models
{
    public class SubOrder
    {
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public Order? Order { get; private set; }
        public int ShopId { get; private set; }
        public Shop? Shop { get; private set; }
        public string? ShopName { get; private set; }
        public decimal TotalAmount { get; private set; }
        public SubOrderStatus SubOrderStatus { get; private set; } = SubOrderStatus.Pending;
        public Payment? Payment { get; set; }
        public ICollection<OrderItem> Items => _items.AsReadOnly();
        private readonly List<OrderItem> _items = new List<OrderItem>();

        private SubOrder() { }

        public static SubOrder Create(int orderId, int shopId, string shopName)
        {
            if (string.IsNullOrWhiteSpace(shopName)) throw new Exceptions.DomainException("Tên shop không được để trống");

            return new SubOrder
            {
                OrderId = orderId,
                ShopId = shopId,
                ShopName = shopName,
                SubOrderStatus = SubOrderStatus.Pending
            };
        }

        public void AddItem(string imageUrl, string productName, int productId, decimal price, int quantity)
        {
            if (IsExistItem(productId))
            {
                var item = FindOrderItem(productId);
                item!.IncreasingQuantity(quantity);
                CalculateTotal();
                return;
            }
            var orderItem = OrderItem.Create(imageUrl, productName, productId, price, quantity);
            _items.Add(orderItem);
            CalculateTotal();
        }

        public void RemoveItem(OrderItem item)
        {
            _items.Remove(item);
            CalculateTotal();
        }

        public OrderItem? FindOrderItem(int productId)
        {
            return Items.FirstOrDefault(x => x.ProductId == productId);
        }

        public void UpdateStatus(SubOrderStatus status)
        {
            SubOrderStatus = status;
        }

        private bool IsExistItem(int productId)
        {
            return Items.Any(x => x.ProductId == productId);
        }

        private void CalculateTotal()
        {
            TotalAmount = Items.Sum(x => x.Quantity * x.Price);
        }
    }
}