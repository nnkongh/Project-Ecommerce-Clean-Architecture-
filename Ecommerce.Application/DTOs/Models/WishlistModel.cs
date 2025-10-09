using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public sealed record WishlistModel  : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public IReadOnlyList<ItemWishlistModel> Items { get; set; } = new List<ItemWishlistModel>();
    }

}
