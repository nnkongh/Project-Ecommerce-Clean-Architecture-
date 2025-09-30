using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.DTOs.Product
{
    public record OrderModel : BaseModel
    {
        public AddressModel Address { get; set; }
        public IReadOnlyList<OrderItemModel> Items { get; set; } = [];
        public OrderStatus status { get; set; }
    }
}
