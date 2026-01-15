using Ecommerce.Application.DTOs.Models;

namespace Ecommerce.Web.ViewModels
{
    public class CategoryViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? ParentId { get; set; }
        public IReadOnlyList<CategoryViewModel> Children { get; set; } = [];
        public IReadOnlyList<ProductViewModel>? Products { get; set; }
    }
}