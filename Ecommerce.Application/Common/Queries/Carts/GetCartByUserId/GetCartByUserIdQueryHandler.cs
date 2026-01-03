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

namespace Ecommerce.Application.Common.Queries.Carts.GetCartByUserId
{
    public sealed class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, Result<CartModel>>
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepo;
        
        public GetCartByUserIdQueryHandler(ICartRepository cartRepo, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        public async Task<Result<CartModel>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var list = await _cartRepo.GetCartWithItemByUserIdAsync(request.userId);
            if(list == null)
            {
                return Result.Failure<CartModel>(new Error("", "Cart is not found"));
            }
            var mapped = _mapper.Map<CartModel>(list);
            return mapped;
        }
    }
}
