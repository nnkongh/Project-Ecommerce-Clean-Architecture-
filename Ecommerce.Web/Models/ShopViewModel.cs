using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Web.ViewModels
{
    public class ShopViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasShop { get; set; }
    }

    public class ShopRegisterViewModel
    {
        public ShopViewModel? Shop { get; set; }
        public ProductViewModel NewProduct { get; set; } = new();
        public List<ProductViewModel> Products { get; set; } = new();
        public List<SelectListItem> ParentCategories { get; set; } = new();
        public List<CategoryViewModel>? AllCategories { get; set; } = new();
    }
}
