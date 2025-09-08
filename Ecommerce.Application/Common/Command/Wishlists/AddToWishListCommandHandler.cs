using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Ecommerce.Application.Interfaces.Authentication;
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

namespace Ecommerce.Application.Common.Command.Wishlists
{
    public class AddToWishListCommandHandler : IRequestHandler<AddToWishListCommand,Result>
    {
        private readonly IProductRepository _productRepo;
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IMapper _mapper;
        private readonly IUserAuthenticationService _userRepo;
        private readonly IUnitOfWork _uow;
        public AddToWishListCommandHandler(IProductRepository productRepo, IWishlistRepository wishlistRepo, IUnitOfWork uow, IMapper mapper, IUserAuthenticationService useRepo)
        {
            _productRepo = productRepo;
            _wishlistRepo = wishlistRepo;
            _uow = uow;
            _mapper = mapper;
            _userRepo = useRepo;
        }

        public Task Handle(Result request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> Handle(AddToWishListCommand request, CancellationToken cancellationToken)
        {
            var existintgProduct = await _productRepo.GetByIdAsync(request.productId);
            if(existintgProduct == null)
            {
                return Result.Failure(new Error("", $"Product with id {request.productId} is not found"));
            }
            var existingUser = await _userRepo.FindByIdAsync(request.userId);
            if(existingUser == null)
            {
                return Result.Failure(new Error("", $"User does not exist"));
            }
            var existingProductInWishlist = await _wishlistRepo.GetExistingProduct(request.userId,request.productId);
            if (existingProductInWishlist)
            {
                return Result.Failure(new Error("", $"Product with id {request.productId} has already existed in your wishlist!"));
            }
            var wishlist = await _wishlistRepo.GetByUserIdAsync(request.userId);
            if(wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = request.userId,
                    Items = new List<ProductWishList>()
                };
            }
            await _wishlistRepo.AddAsync(wishlist);

            wishlist.Items.Add(new ProductWishList
            {
                ProductId = request.productId,
                WishListId = wishlist.Id,
            });
            await _wishlistRepo.UpdateAsync(wishlist.Id, wishlist,w => w.Items);
            return Result.Success(existingUser);

        }
    }
}
