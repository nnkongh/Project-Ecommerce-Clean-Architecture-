using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Category;
using Ecommerce.Domain.Shared;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class CategoryClient : ICategoryClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public CategoryClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _httpClient = httpClient.CreateClient("ApiClient");
            _mapper = mapper;
        }

        public Task<ApiResponse<CategoryModel>> CreateCategoryAsync(CreateCategoryRequest category)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IReadOnlyList<CategoryViewModel>>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("categories");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<CategoryModel>>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<CategoryViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy category");
            }

            var mapped = _mapper.Map<IReadOnlyList<CategoryViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<CategoryViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<CategoryViewModel>> GetCategoryByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"categories/{id}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryViewModel>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<CategoryViewModel>.Fail(result?.Error?.Message ?? $"Không thể lấy category {id}");
            }

            var mapped = _mapper.Map<CategoryViewModel>(result.Value);

            return ApiResponse<CategoryViewModel>.Success(mapped);
        }

        public Task<ApiResponse<CategoryModel>> GetCategoryByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<CategoryViewModel>> UpdateCategoryAsync(int id, UpdateCategoryRequest category)
        {
            throw new NotImplementedException();
        }
    }
}
