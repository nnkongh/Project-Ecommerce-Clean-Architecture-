using Ecommerce.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public class CategoryDetailModel
    {
        public int? ParentCategoryId { get; set; }
        public int? SelectedCategoryId { get; set; }
        public IReadOnlyList<CategoryWithProductModel> ChildCategories { get; set; }
        public PagedResult<ProductModel> DisplayProducts { get; set; }
    }
    public class CategoryWithProductModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public IReadOnlyList<ProductModel>? Products { get; set; }
    }

    public class CategoryListPageResponse
    {
        public IReadOnlyList<CategoryModel> Categories { get; set; }
        public PagedResult<ProductModel> Products { get; set; }
    }
}
