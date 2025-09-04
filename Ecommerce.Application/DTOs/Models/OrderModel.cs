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
    public class OrderModel : BaseProduct
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrderItemModel> orderItems { get; set; } = [];
        public OrderStatus status { get; set; }

    }
}
