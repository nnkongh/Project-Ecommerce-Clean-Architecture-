using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ModelsRequest.Carts
{
    public sealed record AddToCartRequest(int Id, string? name, decimal? price, int quantity = 1);

}
