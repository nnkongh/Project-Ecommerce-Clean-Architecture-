using AutoMapper;
using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Shops.GetShopById
{
    public sealed class GetShopByIdHandler : IRequestHandler<GetShopByIdQuery, Result<ShopModel>>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;

        public GetShopByIdHandler(IShopRepository shopRepository, IMapper mapper)
        {
            _shopRepository = shopRepository;
            _mapper = mapper;
        }

        public async Task<Result<ShopModel>> Handle(GetShopByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return Result.Failure<ShopModel>(Error.NullValue);
            }

            var shop = await _shopRepository.GetByIdAsync(request.Id);
            if (shop == null)
            {
                return Result.Failure<ShopModel>(new Error("404", "Không tìm thấy cửa hàng"));
            }

            var model = _mapper.Map<ShopModel>(shop);
            return Result.Success(model);
        }
    }
}