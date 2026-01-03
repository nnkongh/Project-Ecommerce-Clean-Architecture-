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

namespace Ecommerce.Application.Common.Queries.Carts.GetCartById
{
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, Result<IReadOnlyList<CartItemModel>>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        public GetCartByIdQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<CartItemModel>>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
            {
                return Result.Failure<IReadOnlyList<CartItemModel>>(new Error("Notfound", "Cart is empty"));
            }
            var cart = await _cartRepository.GetCartWithItemByIdAsync(request.id);
            if (cart == null)
            {
                return Result.Failure<IReadOnlyList<CartItemModel>>(new Error("Notfound", "Cart is empty"));
            }
            var mapped = _mapper.Map<IReadOnlyList<CartItemModel>>(cart.Items);
            return Result.Success(mapped);
        }

    }
}
