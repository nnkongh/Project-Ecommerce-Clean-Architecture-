using Ecommerce.Web.ViewModels;

namespace Ecommerce.Web.Models
{
    public class CategoryProductViewModel
    {
        public int CategoryId { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public int ParentId { get; set; }
    }
}
