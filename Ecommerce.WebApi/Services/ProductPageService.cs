using AutoMapper;
using Ecommerce.Web.Interfaces;
using Ecommerce.Web.ViewModels;

namespace Ecommerce.Web.Services
{
    public class ProductPageService : IProductPageService
    {
        private readonly IWishlistPageService _wishlistService;
        private readonly IMapper _mapper;
        private readonly IProductPageService _productService;
        private readonly ICartPageService _cartService;
        private readonly ICategoryPageService _categoryService;

        public ProductPageService(
            IProductPageService productPageService,
            ICartPageService cartPageService,
            ICategoryPageService categoryPageService,
            IWishlistPageService wishlistPageService,
            IMapper mapper)
        {
            _productService = productPageService;
            _cartService = cartPageService;
            _categoryService = categoryPageService;
            _wishlistService = wishlistPageService;
            _mapper = mapper;
        }
        public async Task AddToCart(string userName, Guid productId)
        {
            await _cartService.AddItem(userName, productId);
        }

        public async Task AddToWishList(string userName, Guid productId)
        {
            await _wishlistService.AddItem(userName, productId);
        }

        public async Task<ProductViewModel> GetProductById(Guid productId)
        {
            var product = await _productService.GetProductById(productId);
            var mapped =  _mapper.Map<ProductViewModel>(product);
            return mapped;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProducts(string productName)
        {
            var products = await _productService.GetProducts(productName);
            return products;
        }

        public Task<IEnumerable<ProductViewModel>> GetProductsByCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
