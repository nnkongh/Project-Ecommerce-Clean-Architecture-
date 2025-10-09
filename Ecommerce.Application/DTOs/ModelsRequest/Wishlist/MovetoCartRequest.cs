using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ModelsRequest.Wishlist
{
    public sealed record MovetoCartRequest
    {
        public int ProductId { get; set; }
    }
}
