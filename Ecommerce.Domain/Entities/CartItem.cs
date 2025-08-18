using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public Cart Cart { get; set; } = default!;
        public int CartId { get; set; }
        public Product Product { get; set; } = default!;
        public int ProductId { get; set; }
    }
}
