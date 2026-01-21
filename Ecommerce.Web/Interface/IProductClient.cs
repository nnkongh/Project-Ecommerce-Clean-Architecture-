using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface IProductClient
    {
        Task<ApiResponse<ProductViewModel>> CreateProductAsync(ProductViewModel product);
        Task<ApiResponse<ProductViewModel>> UpdateProductAsync(int id, ProductViewModel product);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
        Task<ApiResponse<ProductViewModel>> GetProductByIdAsync(int id);
        
        Task<ApiResponse<IReadOnlyList<ProductViewModel>>> GetAllProductsByCategoryAsync(int categoryId);
        Task<ApiResponse<IReadOnlyList<ProductViewModel>>> GetAllProductsByNameAsync(string name);
        Task<ApiResponse<IReadOnlyList<ProductViewModel>>> GetAllProductsAsync();


    }
}
