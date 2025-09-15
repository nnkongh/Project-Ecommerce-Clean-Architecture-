using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ModelsRequest.Wishlist
{
    public record AddToWishlistRequest(int ProductId, string UserId)
    {
    }
}
