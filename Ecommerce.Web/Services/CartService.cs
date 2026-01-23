using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Carts;
using Ecommerce.Application.Interfaces;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Services.Strategy;
using Ecommerce.Web.ViewModels;

namespace Ecommerce.Web.Services
{
    public class CartService
    {
        private readonly ICartStrategyFactory _factory;
        private readonly IHttpContextAccessor _httpContext;

        public CartService(ICartStrategyFactory factory, IHttpContextAccessor httpContext)
        {
            _factory = factory;
            _httpContext = httpContext;
        }
        private ICartStrategy GetCartStrategy => _factory.CreateCartStrategy(_httpContext);
        public Task AddToCartAsync(AddToCartRequest request) => GetCartStrategy.AddToCartAsync(request);
        public Task ClearCartAsync() => GetCartStrategy.ClearCartAsync();
        public Task<CartViewModel> GetCartAsync() => GetCartStrategy.GetCartAsync();
        public Task RemoveFromCartAsync(int productId) => GetCartStrategy.RemoveFromCartAsync(productId);
        public Task UpdateCartAsync(int productId, int quantity) => GetCartStrategy.UpdateCartAsync(productId, quantity);
    }
}
