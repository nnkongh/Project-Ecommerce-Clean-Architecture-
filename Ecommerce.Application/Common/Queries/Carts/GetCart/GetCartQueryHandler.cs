using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Carts.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetItemQuery, Result<IReadOnlyList<CartItemModel>>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        public GetCartQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<CartItemModel>>> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
            {
                return Result.Failure<IReadOnlyList<CartItemModel>>(new Error("Notfound", "Cart is empty"));
            }
            var cart = await _cartRepository.GetCartByIdAsync(request.id);
            if (cart == null)
            {
                return Result.Failure<IReadOnlyList<CartItemModel>>(new Error("Notfound", "Cart is empty"));
            }
            var mapped = _mapper.Map<IReadOnlyList<CartItemModel>>(cart.Items);
            return Result.Success(mapped);
        }

    }
}
