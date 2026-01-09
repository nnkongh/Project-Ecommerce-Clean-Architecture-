using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public record class CategoryModel : BaseModel
    {
        public string? Name { get; set; }

        public int? ParentId { get; set; }
        [JsonIgnore]
        public ICollection<CategoryModel> Children { get; set; } = [];

        [JsonIgnore]
        public List<ProductModel>? Products { get; set; }
    }
}
