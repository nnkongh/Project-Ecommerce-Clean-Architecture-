using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Category;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface ICategoryClient
    {
        Task<ApiResponse<IReadOnlyList<CategoryViewModel>>> GetRootCategoriesAsync();
        Task<ApiResponse<CategoryModel>> GetCategoryByNameAsync(string name);
        Task<ApiResponse<CategoryModel>> CreateCategoryAsync(CreateCategoryRequest category);
        Task<ApiResponse<CategoryViewModel>> UpdateCategoryAsync(int id, UpdateCategoryRequest category);
        Task<ApiResponse<IReadOnlyList<CategoryViewModel>>> GetChildCategoriesAsync(int parentId);
        Task<ApiResponse<IReadOnlyList<CategoryViewModel>>> GetAllCategoriesAsync();
        Task<ApiResponse<CategoryViewModel>> GetCategoryByIdAsync(int? id);


    }
}
