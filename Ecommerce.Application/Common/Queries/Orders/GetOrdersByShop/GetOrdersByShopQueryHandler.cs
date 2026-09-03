using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Orders.GetOrdersByShop
{
    public sealed class GetOrdersByShopQueryHandler : IRequestHandler<GetOrdersByShopQuery, Result<IReadOnlyList<OrderModel>>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IShopRepository _shopRepo;
        private readonly IMapper _mapper;

        public GetOrdersByShopQueryHandler(IOrderRepository orderRepo, IShopRepository shopRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _shopRepo = shopRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<OrderModel>>> Handle(GetOrdersByShopQuery request, CancellationToken cancellationToken)
        {
            var shop = await _shopRepo.GetByUserIdAsync(request.UserId);
            if (shop == null)
            {
                return Result.Failure<IReadOnlyList<OrderModel>>(new Error("404", "Bạn chưa có cửa hàng"));
            }

            var orders = await _orderRepo.GetOrdersByShopIdAsync(shop.Id);
            if (orders == null || !orders.Any())
            {
                return Result.Failure<IReadOnlyList<OrderModel>>(new Error("404", "Không tìm thấy đơn hàng nào cho cửa hàng này"));
            }

            var mapped = _mapper.Map<IReadOnlyList<OrderModel>>(orders);
            return Result.Success(mapped);
        }
    }
}
