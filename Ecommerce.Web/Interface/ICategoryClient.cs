using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Category;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface ICategoryClient
    {
        Task<ApiResponse<IReadOnlyList<CategoryViewModel>>> GetCategoriesAsync();
        Task<ApiResponse<CategoryModel>> GetCategoryByNameAsync(string name);
        Task<ApiResponse<CategoryModel>> CreateCategoryAsync(CreateCategoryRequest category);
        Task<ApiResponse<CategoryViewModel>> UpdateCategoryAsync(int id, UpdateCategoryRequest category);

    }
}
