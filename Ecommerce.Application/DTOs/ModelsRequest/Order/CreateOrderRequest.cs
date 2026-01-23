using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ModelsRequest.Order
{
    public class CreateOrderRequest
    {
        public CartModel Cart { get; set; } 
        public UserModel User { get; set; }
    }
}
