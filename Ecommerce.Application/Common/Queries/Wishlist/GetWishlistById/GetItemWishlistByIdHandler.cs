using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Wishlist.GetWishlistById
{
    public sealed class GetItemWishlistByIdHandler : IRequestHandler<GetItemWishlistByIdQuery,Result<WishlistModel>>
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IMapper _mapper;
        public GetItemWishlistByIdHandler(IWishlistRepository wishlistRepo, IMapper mapper)
        {
            _wishlistRepo = wishlistRepo;
            _mapper = mapper;
        }

        public async Task<Result<WishlistModel>> Handle(GetItemWishlistByIdQuery request, CancellationToken cancellationToken)
        {
            var wishlist = await _wishlistRepo.GetWishlistWithItemByIdAsync(request.wishlistId);
            if(wishlist == null)
            {
                return Result.Failure<WishlistModel>(new Error("", "Wishlist không tồn tại"));
            }
            var mapped = _mapper.Map<WishlistModel>(wishlist);
            return Result.Success(mapped);
        }
    }
}
