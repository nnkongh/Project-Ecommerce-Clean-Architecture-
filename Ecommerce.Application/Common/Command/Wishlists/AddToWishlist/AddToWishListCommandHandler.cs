using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

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
                return Result.Failure<WishlistModel>(new Error("", $"Product with id {Command.Request.ProductId} is not found"));
            }
            var existingUser = await _userRepo.GetByIdAsync(Command.userId);
            if (existingUser == null)
            {
                return Result.Failure<WishlistModel>(new Error("", $"User does not exist"));
            }
            var wishlist = await _wishlistRepo.GetWishlistWithItemByIdAsync(Command.Request.wishlistId);

            if (wishlist != null)
            {
                var existingItem = wishlist.Items.FirstOrDefault(x => x.ProductId == Command.Request.ProductId);
                if (existingItem != null)
                {
                    return Result.Failure<WishlistModel>(new Error("", $"Item {product.Name} has already exist in your wishlist"));
                }
                wishlist.AddItem(Command.Request.ProductId, product.Name);
                await _uow.SaveChangesAsync(cancellationToken);
                var mapped = _mapper.Map<WishlistModel>(wishlist);
                return Result.Success(mapped);
            }
            else
            {
                wishlist = Wishlist.Create(Command.userId);
                wishlist.AddItem(Command.Request.ProductId, product.Name);
                await _wishlistRepo.AddAsync(wishlist);
                await _uow.SaveChangesAsync(cancellationToken);
                var mapped = _mapper.Map<WishlistModel>(wishlist);
                return Result.Success(mapped);
            }
        }
    }
}
