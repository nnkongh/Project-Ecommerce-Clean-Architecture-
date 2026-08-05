using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Wishlist.GetWishlistsByUserId
{
    public sealed class GetWishlistsByUserIdQueryHandler : IRequestHandler<GetWishlistsByUserIdQuery, Result<IReadOnlyList<WishlistModel>>>
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IMapper _mapper;
        public GetWishlistsByUserIdQueryHandler(IWishlistRepository wishlistRepo, IMapper mapper)
        {
            _wishlistRepo = wishlistRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<WishlistModel>>> Handle(GetWishlistsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var list = await _wishlistRepo.GetWishlistsWithItemByUserIdAsync(request.userId);
            var mapped = _mapper.Map<IReadOnlyList<WishlistModel>>(list ?? new List<Ecommerce.Domain.Models.Wishlist>());
            return Result.Success(mapped);
        }
    }
}
