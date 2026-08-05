using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;

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
            if (user == null) return Result.Failure<CartModel>(new Error("", "Người dùng không tồn tại"));

            var wishlist = await _wishlistRepo.GetWishlistWithItemByIdAsync(request.wishlistId);
            if (wishlist == null) return Result.Failure<CartModel>(new Error("", "Wishlist không tồn tại"));

            var product = await _productRepo.GetByIdAsync(request.request.ProductId);
            if (product == null) return Result.Failure<CartModel>(new Error("", "Sản phẩm không tồn tại"));

            var wishlistItem = wishlist.Items.FirstOrDefault(x => x.ProductId == request.request.ProductId);
            if (wishlistItem == null) return Result.Failure<CartModel>(new Error("", "Sản phẩm không tồn tại trong wishlist"));

            var cart = await _cartRepo.GetCartWithItemByUserIdAsync(request.userId);
            if (cart == null)
            {
                cart = Cart.CreateCart(request.userId);
                await _cartRepo.AddAsync(cart);
            }
            cart.AddItem(product.Id, wishlistItem.ProductName!, 1, product.Price, product.ImageUrl);
            wishlist.RemoveItem(request.request.ProductId);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<CartModel>(cart);
            return Result.Success(mapped);
        }
    }
}
