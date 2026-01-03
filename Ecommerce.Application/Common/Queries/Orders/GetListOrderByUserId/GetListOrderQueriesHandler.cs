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

namespace Ecommerce.Application.Common.Queries.Orders.GetListOrderByUserId
{
    public sealed class GetListOrderQueriesHandler : IRequestHandler<GetListOrderQuery, Result<IReadOnlyList<OrderModel>>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public GetListOrderQueriesHandler(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }


        public async Task<Result<IReadOnlyList<OrderModel>>> Handle(GetListOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepo.GetOrdersByUserIdAsync(request.userId);
            if (order == null) {
                return Result.Failure<IReadOnlyList<OrderModel>>(new Error("", "User not found"));            
            }
            var mapped = _mapper.Map<IReadOnlyList<OrderModel>>(order);
            return Result.Success(mapped);

        }
    }
}
