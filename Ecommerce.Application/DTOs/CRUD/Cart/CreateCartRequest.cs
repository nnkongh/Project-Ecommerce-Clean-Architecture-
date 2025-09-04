using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.CRUD.Cart
{
    public sealed record CreateCartRequest(int UserId, List<CartItemModel> Items, int Id)
    {
    }
}
