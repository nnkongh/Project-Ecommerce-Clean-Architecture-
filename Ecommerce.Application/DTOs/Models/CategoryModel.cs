using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Product
{
    public record class CategoryModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public List<ProductModel>? Products { get; set; }
    }
}
