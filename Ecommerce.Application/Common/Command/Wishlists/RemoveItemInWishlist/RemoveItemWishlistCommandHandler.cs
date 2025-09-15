using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Wishlists.RemoveItemInWishlist
{
    public sealed class RemoveItemWishlistCommandHandler : IRequestHandler<RemoveItemWishlistCommand, Result>
    {
        private readonly IUserRepository _userRepo;
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IUnitOfWork _uow;
        public RemoveItemWishlistCommandHandler(IUserRepository userRepo, IWishlistRepository wishlistRepo, IUnitOfWork uow)
        {
            _userRepo = userRepo;
            _wishlistRepo = wishlistRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(RemoveItemWishlistCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.userId);
            if (user == null)
            {
                return Result.Failure<UserModel>(new Error("", "User is not found"));
            }
            var wishlist = await _wishlistRepo.GetWishlistByUserIdAsync(user.Id);
            if (wishlist == null)
            {
                return Result.Failure(new Error("", "wishlist is empty"));
            }
            wishlist.RemoveItem(request.productId);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
