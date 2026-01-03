using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Wishlists.MoveItemToCart
{
    public sealed class MoveItemToCartCommandHandler : IRequestHandler<MoveItemToCartCommand, Result<CartModel>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IWishlistRepository _wishlistRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IUnitOfWork _uow;
        private readonly IProductRepository _productRepo;
        public MoveItemToCartCommandHandler(IUserRepository userRepo, IUnitOfWork uow, ICartRepository cartRepo, IWishlistRepository wishlistRepo, IMapper mapper, IProductRepository productRepo)
        {
            _userRepo = userRepo;
            _uow = uow;
            _cartRepo = cartRepo;
            _wishlistRepo = wishlistRepo;
            _mapper = mapper;
            _productRepo = productRepo;
        }

        public async Task<Result<CartModel>> Handle(MoveItemToCartCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.userId);
            if (user == null) return Result.Failure<CartModel>(new Error("", "User not found"));


            var wishlist = await _wishlistRepo.GetWishlistWithItemByIdAsync(request.wishlistId);
            if (wishlist == null) return Result.Failure<CartModel>(new Error("", "Wishlist not found"));

            var product = await _productRepo.GetByIdAsync(request.request.ProductId);
            if (product == null) return Result.Failure<CartModel>(new Error("", "Product not found"));

            var wishlistItem = wishlist.Items.FirstOrDefault(x => x.ProductId == request.request.ProductId);
            if(wishlistItem == null) return Result.Failure<CartModel>(new Error("", "Product not found in wishlist"));

            var cart = await _cartRepo.GetCartWithItemByUserIdAsync(request.userId);
            if (cart == null)
            {
                cart = Cart.CreateCart(request.userId);
                await _cartRepo.AddAsync(cart);
            }
            cart.AddItemFromWishlist(product.Id, wishlistItem.ProductName!, product.Price);
            wishlist.RemoveItem(wishlistItem.Id);
            await _wishlistRepo.Delete(wishlist);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<CartModel>(cart);
            return Result.Success(mapped);
        }
    }
}
