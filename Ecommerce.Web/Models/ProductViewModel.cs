using Ecommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int? ParentCategoryId { get; set; }
        public int? ChildCategoryId { get; set; }
        public List<SelectListItem> ParentCategories { get; set; } = new();
        public List<CategoryViewModel>? AllCategories { get; set; } = new();
    }
}
