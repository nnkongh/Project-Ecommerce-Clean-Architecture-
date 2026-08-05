using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Product;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ProductClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<ProductViewModel>> CreateProductAsync(ProductViewModel product)
        {
            var response = await _httpClient.PostAsJsonAsync("products", product);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ProductViewModel>.Fail(result?.Error?.Message ?? "Không thể tạo mới sản phẩm");
            }

            var mapped = _mapper.Map<ProductViewModel>(result.Value);

            return ApiResponse<ProductViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<bool>.Fail("Không thể xóa sản phẩm");
            }
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<IReadOnlyList<ProductViewModel>>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetAsync("products/list");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ProductModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<ProductViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy danh sách sản phẩm");
            }

            var mapped = _mapper.Map<IReadOnlyList<ProductViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<ProductViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<IReadOnlyList<ProductViewModel>>> GetAllProductsByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync($"products/category/{categoryId}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ProductModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<ProductViewModel>>.Fail(
                result?.Error?.Message ?? "Không thể lấy danh sách sản phẩm");
            }
            var mapped = _mapper.Map<IReadOnlyList<ProductViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<ProductViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<IReadOnlyList<ProductViewModel>>> GetAllProductsByNameAsync(string name)
        {
            var response = await _httpClient.GetAsync($"products/search?name={Uri.EscapeDataString(name)}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ProductModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<ProductViewModel>>.Fail(
                result?.Error?.Message ?? "Không thể lấy danh sách sản phẩm");
            }

            var mapped = _mapper.Map<IReadOnlyList<ProductViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<ProductViewModel>>.Success(mapped);
        }

        public async Task<PagedResult<ProductViewModel>> GetAllProductsByPaginationAsync(int page, int pageSize, string? sortBy = null, decimal? minPrice = null, decimal? maxPrice = null, int? categoryId = null, string? searchTerm = null)
        {
            var queryParams = new List<string>
            {
                $"page={page}",
                $"pageSize={pageSize}"
            };
            if (!string.IsNullOrEmpty(sortBy)) queryParams.Add($"sortBy={sortBy}");
            if (minPrice.HasValue) queryParams.Add($"minPrice={minPrice.Value}");
            if (maxPrice.HasValue) queryParams.Add($"maxPrice={maxPrice.Value}");
            if (categoryId.HasValue) queryParams.Add($"categoryId={categoryId.Value}");
            if (!string.IsNullOrEmpty(searchTerm)) queryParams.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");

            var response = await _httpClient.GetAsync($"products/items-paginated?{string.Join("&", queryParams)}");

            var result = await response.Content.ReadFromJsonAsync<PagedResult<ProductModel>>();

            return result is null
                ? new PagedResult<ProductViewModel>(Array.Empty<ProductViewModel>(), 0, page, pageSize)
                : _mapper.Map<PagedResult<ProductViewModel>>(result);
        }

        public async Task<ApiResponse<ProductViewModel>> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"products/item/{id}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ProductViewModel>.Fail(result?.Error?.Message ?? "Không thể lấy sản phẩm");
            }

            var mapped = _mapper.Map<ProductViewModel>(result.Value);

            return ApiResponse<ProductViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<ProductViewModel>> UpdateProductAsync(int id, ProductViewModel product)
        {
            var response = await _httpClient.PatchAsJsonAsync($"products/{id}", product);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ProductViewModel>.Fail(result?.Error?.Message ?? "Không thể cập nhật sản phẩm");
            }

            var mapped = _mapper.Map<ProductViewModel>(result.Value);

            return ApiResponse<ProductViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<IReadOnlyList<ProductViewModel>>> GetFilteredProductsAsync(
            string? sortBy = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? categoryId = null,
            string? searchTerm = null)
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(sortBy)) queryParams.Add($"sortBy={sortBy}");
            if (minPrice.HasValue) queryParams.Add($"minPrice={minPrice.Value}");
            if (maxPrice.HasValue) queryParams.Add($"maxPrice={maxPrice.Value}");
            if (categoryId.HasValue) queryParams.Add($"categoryId={categoryId.Value}");
            if (!string.IsNullOrEmpty(searchTerm)) queryParams.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");

            var query = queryParams.Count > 0 ? $"?{string.Join("&", queryParams)}" : "";
            var response = await _httpClient.GetAsync($"products/filtered{query}");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ProductModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<ProductViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy danh sách sản phẩm");
            }

            var mapped = _mapper.Map<IReadOnlyList<ProductViewModel>>(result.Value);
            return ApiResponse<IReadOnlyList<ProductViewModel>>.Success(mapped);
        }
    }
}
