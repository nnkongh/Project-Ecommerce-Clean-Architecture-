using Ecommerce.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public record WishlistModel(int ProductId, string UserId, IReadOnlyList<ItemWishlistModel> List) : BaseModel;
}
