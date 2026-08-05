using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Wishlists.AddToWishlist
{
    public sealed class AddToWishListCommandHandler : IRequestHandler<AddToWishListCommand, Result<WishlistModel>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public AddToWishListCommandHandler(IProductRepository productRepo, IWishlistRepository wishlistRepo, IUnitOfWork uow, IMapper mapper, IUserRepository userRepo)
        {
            _productRepo = productRepo;
            _wishlistRepo = wishlistRepo;
            _uow = uow;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public async Task<Result<WishlistModel>> Handle(AddToWishListCommand Command, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetByIdAsync(Command.Request.ProductId);
            if (product == null)
            {
                return Result.Failure<WishlistModel>(new Error("", $"Sản phẩm với ID {Command.Request.ProductId} không tồn tại"));
            }
            var existingUser = await _userRepo.GetByIdAsync(Command.UserId);
            if (existingUser == null)
            {
                return Result.Failure<WishlistModel>(new Error("", $"Người dùng không tồn tại"));
            }

            var wishlist = await _wishlistRepo.GetWishlistWithItemByUserId(Command.UserId);

            if (wishlist != null)
            {
                var existingItem = wishlist.Items.FirstOrDefault(x => x.ProductId == Command.Request.ProductId);
                if (existingItem != null)
                {
                    return Result.Failure<WishlistModel>(new Error("", $"Sản phẩm {product.Name} đã tồn tại trong wishlist"));
                }
                wishlist.AddItem(Command.Request.ProductId, product.Name, product.ImageUrl);
                await _uow.SaveChangesAsync(cancellationToken);
                var mapped = _mapper.Map<WishlistModel>(wishlist);
                return Result.Success(mapped);
            }
            else
            {
                wishlist = Wishlist.Create(Command.UserId);
                wishlist.AddItem(Command.Request.ProductId, product.Name, product.ImageUrl);
                await _wishlistRepo.AddAsync(wishlist);
                await _uow.SaveChangesAsync(cancellationToken);
                var mapped = _mapper.Map<WishlistModel>(wishlist);
                return Result.Success(mapped);
            }
        }
    }
}
