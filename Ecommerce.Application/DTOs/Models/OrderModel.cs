using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public record OrderModel : BaseModel
    {
        public AddressModel Address { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public IReadOnlyList<OrderItemModel> Items { get; set; } = [];
        public OrderStatus OrderStatus { get; set; }
    }
}
