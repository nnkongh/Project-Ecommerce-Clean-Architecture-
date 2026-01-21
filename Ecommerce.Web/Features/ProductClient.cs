using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Product;
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

            if(result == null || !result.IsSuccess)
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

        public async Task<ApiResponse<IReadOnlyList<ProductViewModel>>> GetAllProductsByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync($"products/category/{categoryId}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ProductModel>>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<ProductViewModel>>.Fail(
                result?.Error?.Message ?? "Không thể lấy danh sách sản phẩm");
            }
            var mapped = _mapper.Map<IReadOnlyList<ProductViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<ProductViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<ProductViewModel>> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"products/item/{id}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductModel>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<ProductViewModel>.Fail(result?.Error?.Message ?? "Không thể lấy sản phẩm");
            }

            var mapped = _mapper.Map<ProductViewModel>(result.Value);

            return ApiResponse<ProductViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<ProductViewModel>> UpdateProductAsync(int id, ProductViewModel product)
        {
            //var updateModelt = _mapper.Map<UpdateProductRequest>(product);
            var response = await _httpClient.PatchAsJsonAsync($"products/{id}", product);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ProductViewModel>.Fail(result?.Error?.Message ?? "Không thể cập nhật sản phẩm");
            }

            var mapped = _mapper.Map<ProductViewModel>(result.Value);

            return ApiResponse<ProductViewModel>.Success(mapped);
        }
    }
}
