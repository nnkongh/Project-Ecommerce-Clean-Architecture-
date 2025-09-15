using Ecommerce.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public record CartItemModel : BaseModel
    {
        public string? ProductName { get; set; } 
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
        public decimal UnitPrice { get; set; }
    }
}
