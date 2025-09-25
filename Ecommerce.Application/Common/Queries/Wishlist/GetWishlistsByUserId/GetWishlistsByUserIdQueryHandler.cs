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

namespace Ecommerce.Application.Common.Queries.Wishlist.GetWishlistsByUserId
{
    public sealed class GetWishlistsByUserIdQueryHandler : IRequestHandler<GetWishlistsByUserIdQuery, Result<IReadOnlyList<WishlistModel>>>
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public GetWishlistsByUserIdQueryHandler(IUserRepository userRepo, IWishlistRepository wishlistRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _wishlistRepo = wishlistRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<WishlistModel>>> Handle(GetWishlistsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.userId);
            if (user == null) {
                return Result.Failure<IReadOnlyList<WishlistModel>>(new Error("", "User not found"));
            }
            var list = await _wishlistRepo.GetWishlistsWithItemByUserIdAsync(request.userId);
            if(list == null)
            {
                return Result.Failure<IReadOnlyList<WishlistModel>>(new Error("", "You dont have any wishlist list"));
            }
            var mapped = _mapper.Map<IReadOnlyList<WishlistModel>>(list);
            return Result.Success(mapped);
        }
    }
}
