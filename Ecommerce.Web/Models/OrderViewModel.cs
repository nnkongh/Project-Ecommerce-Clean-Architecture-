using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Models;

namespace Ecommerce.Web.ViewModels
{
    public record OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; } //
        public string CustomerName { get; set; } = null!;
        public Address? Address { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    }
}
