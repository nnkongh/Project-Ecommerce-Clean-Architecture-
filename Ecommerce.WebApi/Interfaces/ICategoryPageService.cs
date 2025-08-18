using Ecommerce.Web.ViewModels;

namespace Ecommerce.Web.Interfaces
{
    public interface ICategoryPageService
    {
        Task<IEnumerable<CategoryViewModel>> GetCategories();
    }
}
