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
        public decimal TotalPrice => Quantity * UnitPrice;
        public decimal UnitPrice { get; set; }
        public Cart Cart { get; set; } = default!;
        public int CartId { get; set; }
        public Product Product { get; set; } = default!;
        public int ProductId { get; set; }

        public void IncreaseQuantity(int qty) => Quantity += qty;
        public void DecreaseQuantity(int qty)
        {
            if(qty >= Quantity)
            {
                Quantity = 0;
            }
            else
            {
                Quantity -= qty;
            }
        }
    }
}
