using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Wishlist.GetItemWishlist
{
    public sealed class GetItemWishlistByIdHandler : IRequestHandler<GetItemWishlistByIdQuery,Result<IReadOnlyList<ItemWishlistModel>>>
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IMapper _mapper;
        public GetItemWishlistByIdHandler(IWishlistRepository wishlistRepo, IMapper mapper)
        {
            _wishlistRepo = wishlistRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<ItemWishlistModel>>> Handle(GetItemWishlistByIdQuery request, CancellationToken cancellationToken)
        {
            var wishlist = await _wishlistRepo.GetWishlistWithItemByIdAsync(request.wishlistId);
            if(wishlist == null)
            {
                return Result.Failure<IReadOnlyList<ItemWishlistModel>>(new Error("", "Wishlist is empty"));
            }
            if(wishlist.Items.Count == 0 || wishlist.Items == null)
            {
                return Result.Success<IReadOnlyList<ItemWishlistModel>>(new List<ItemWishlistModel>());
            }
            var mapped = _mapper.Map<IReadOnlyList<ItemWishlistModel>>(wishlist.Items);
            return Result.Success(mapped);

        }
    }
}
