using Ecommerce.Web.ViewModels;

namespace Ecommerce.Web.Interfaces
{
    public interface IProductPageService
    {
        Task<IEnumerable<ProductViewModel>> GetProducts(string productName);
        Task<ProductViewModel> GetProductById(Guid productId);
        Task<IEnumerable<ProductViewModel>> GetProductsByCategory(Guid categoryId);
        Task AddToCart(string userName, Guid productId);
        Task AddToWishList(string userName, Guid productId);
    }
}
