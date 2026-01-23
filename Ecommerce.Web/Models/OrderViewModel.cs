using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Ecommerce.Web.ViewModels
{
    public record OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; } //
        public string CustomerName { get; set; } = null!;
        public string? Email { get; set;}
        public string? PhoneNumber { get; set; }
        public Address? Address { get; set; }
        public IReadOnlyList<OrderItemViewModel> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    }
}
