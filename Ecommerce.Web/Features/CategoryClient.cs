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

        public async Task<ApiResponse<IReadOnlyList<CategoryViewModel>>> GetRootCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("categories/root");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<CategoryModel>>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<CategoryViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy category");
            }

            var mapped = _mapper.Map<IReadOnlyList<CategoryViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<CategoryViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<IReadOnlyList<CategoryViewModel>>> GetChildCategoriesAsync(int id)
        {
            var response = await _httpClient.GetAsync($"categories/{id}/children");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<CategoryViewModel>>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<CategoryViewModel>>.Fail(result?.Error?.Message ?? $"Không thể lấy category {id}");
            }

            var mapped = _mapper.Map<IReadOnlyList<CategoryViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<CategoryViewModel>>.Success(mapped);
        }

        public Task<ApiResponse<CategoryModel>> GetCategoryByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<CategoryViewModel>> UpdateCategoryAsync(int id, UpdateCategoryRequest category)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IReadOnlyList<CategoryViewModel>>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("categories/all");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<CategoryModel>>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<CategoryViewModel>>.Fail(result?.Error?.Message ?? $"Không thể lấy category");
            }

            var mapped = _mapper.Map<IReadOnlyList<CategoryViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<CategoryViewModel>>.Success(mapped);

        }

        public async Task<ApiResponse<PagedResult<CategoryViewModel>>> GetRootCategoriesPagedAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"categories/root/paged?page={page}&pageSize={pageSize}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<CategoryModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<PagedResult<CategoryViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy danh mục");
            }

            var mapped = _mapper.Map<PagedResult<CategoryViewModel>>(result.Value);

            return ApiResponse<PagedResult<CategoryViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<PagedResult<ProductViewModel>>> GetChildCategoriesPagedAsync(int parentId, int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"categories/{parentId}/children/paged?page={page}&pageSize={pageSize}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<ProductModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<PagedResult<ProductViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy danh mục con");
            }

            var mapped = _mapper.Map<PagedResult<ProductViewModel>>(result.Value);

            return ApiResponse<PagedResult<ProductViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<CategoryDetailModel>> GetCategoryDetailAsync(int parentId, int? selectedCategoryId)
        {
            var url = $"categories/{parentId}/details";
            if (selectedCategoryId.HasValue)
            {
                url += $"?selectedCategoryId={selectedCategoryId.Value}";
            }

            var response = await _httpClient.GetAsync(url);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryDetailModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<CategoryDetailModel>.Fail(result?.Error?.Message ?? "Không thể lấy chi tiết danh mục");
            }

            return ApiResponse<CategoryDetailModel>.Success(result.Value!);
        }

        public async Task<ApiResponse<CategoryViewModel>> GetCategoryByIdAsync(int? id)
        {
            var response = await _httpClient.GetAsync($"categories/{id}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<CategoryViewModel>.Fail(result?.Error?.Message ?? $"Không thể lấy category");
            }

            var mapped = _mapper.Map<CategoryViewModel>(result.Value);

            return ApiResponse<CategoryViewModel>.Success(mapped);
        }
    }
}
