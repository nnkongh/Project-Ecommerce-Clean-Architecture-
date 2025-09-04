using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public class CartItemModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } 
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
