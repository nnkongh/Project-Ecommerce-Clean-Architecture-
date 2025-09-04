using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.CRUD.Order
{
    public class CreateOrderRequest
    {
        IEnumerable<OrderItemModel> Items { get; set; } = [];
        public string? UserId { get; set; }
    }
}
