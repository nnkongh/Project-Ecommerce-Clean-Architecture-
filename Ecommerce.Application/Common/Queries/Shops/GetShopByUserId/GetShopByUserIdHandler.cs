using AutoMapper;
using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Shops.GetShopByUserId
{
    public sealed class GetShopByUserIdHandler : IRequestHandler<GetShopByUserIdQuery, Result<ShopModel>>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;

        public GetShopByUserIdHandler(IShopRepository shopRepository, IMapper mapper)
        {
            _shopRepository = shopRepository;
            _mapper = mapper;
        }

        public async Task<Result<ShopModel>> Handle(GetShopByUserIdQuery request, CancellationToken cancellationToken)
        {
            var shop = await _shopRepository.GetByUserIdAsync(request.UserId);
            if (shop == null)
            {
                return Result.Failure<ShopModel>(new Error("404", "Bạn chưa có cửa hàng"));
            }

            var model = _mapper.Map<ShopModel>(shop);
            return Result.Success(model);
        }
    }
}
