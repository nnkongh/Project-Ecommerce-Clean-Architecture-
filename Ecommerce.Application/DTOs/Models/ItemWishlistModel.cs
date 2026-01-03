using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public record ItemWishlistModel(int productId, string productName,int wishlistId) : BaseModel
    {
    }
}
