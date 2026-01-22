using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public record class CartModel : BaseModel
    {
        public IReadOnlyList<CartItemModel> Items { get; set; } = [];
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpiredAt { get; set; } = DateTime.Today.AddDays(4);
    }
}
