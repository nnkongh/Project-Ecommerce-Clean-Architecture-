using Ecommerce.Domain.Models;

namespace Ecommerce.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}
