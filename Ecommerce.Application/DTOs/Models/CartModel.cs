using Ecommerce.Application.DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Product
{
    public record class CartModel : BaseModel
    {
        public IReadOnlyList<CartItemModel> Items { get; set; } = [];
        public string UserId { get; set; }
    }
}
