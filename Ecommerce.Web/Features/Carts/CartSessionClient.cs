using AutoMapper;
using Ecommerce.Application.DTOs.ModelsRequest.Cart;
using Ecommerce.Domain.Models;
using Ecommerce.Web.Extensions;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Services.Strategy;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;
using Microsoft.AspNetCore.Http.Features;

namespace Ecommerce.Web.Features.Carts
{
    public class CartSessionClient : ICartStrategy
    {
        private readonly IHttpContextAccessor _httpContext;
        private const string CartSessionKey = "ShoppingCart";
        private readonly IProductClient _productClient;
        public CartSessionClient(IHttpClientFactory httpClient, IHttpContextAccessor httpContext, IProductClient productClient)
        {
            _httpContext = httpContext;
            _productClient = productClient;
        }
        private ISession Session => _httpContext.HttpContext?.Session ?? throw new InvalidOperationException("Httpcontext is not available");

        public Task<CartViewModel> GetCartAsync()
        {
            var cart = Session.GetObject<CartViewModel>(CartSessionKey);
            if (cart == null)
            {
                cart = new CartViewModel
                {
                    Items = new List<CartItemViewModel>()
                };
                Session.SetObject(CartSessionKey, cart);
            }
            return Task.FromResult(cart);
        }

        public async Task AddToCartAsync(AddToCartRequest product)
        {
            var cart = Session.GetObject<CartViewModel>(CartSessionKey) ?? new CartViewModel { Items = new List<CartItemViewModel>() };

            var productDb = await _productClient.GetProductByIdAsync(product.Id);

            if(productDb == null || productDb.IsSuccess == false)
            {
                throw new Exception("Product not found");
            }

            var existing = cart.Items.FirstOrDefault(x => x.ProductId == product.Id);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    Name = productDb.Value.Name!,
                    TotalPrice = productDb.Value.Price,
                    ImageUrl = productDb.Value.ImageUrl!
                });
            }

            Session.SetObject(CartSessionKey, cart);
        }

        public Task RemoveFromCartAsync(int productId)
        {
            var cart = Session.GetObject<CartViewModel>(CartSessionKey);

            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    Session.SetObject(CartSessionKey, cart);
                }
            }
            return Task.CompletedTask;

        }

        public Task ClearCartAsync()
        {
            Session.Remove(CartSessionKey);
            return Task.CompletedTask;
        }

        public Task UpdateCartAsync(int productId, int quantity)
        {
            var cart = Session.GetObject<CartViewModel>(CartSessionKey);

            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

                if (item != null)
                {
                    if (item.Quantity <= 0)
                    {
                        cart.Items.Remove(item);
                    }
                    else
                    {
                        item.Quantity -= quantity;
                    }
                    Session.SetObject(CartSessionKey, cart);
                }

            }
            return Task.CompletedTask;
        }
    }
}
