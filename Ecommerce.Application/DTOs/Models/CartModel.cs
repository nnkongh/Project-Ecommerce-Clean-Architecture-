using Ecommerce.Application.DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Product
{
    public class CartModel
    {
        public int Id { get; set; }
        public List<CartItemModel> Items { get; set; } = [];
        public string UserId { get; set; }
    }
}
