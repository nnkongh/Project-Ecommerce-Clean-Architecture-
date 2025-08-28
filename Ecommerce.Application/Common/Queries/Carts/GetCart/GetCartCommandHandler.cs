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
    public class GetCartCommandHandler : IRequestHandler<GetItemQueries, Result<IReadOnlyList<CartItemModel>>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        public GetCartCommandHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<CartItemModel>>> Handle(GetItemQueries request, CancellationToken cancellationToken)
        {
            if(request.cart.Id <= 0)
            {
                return Result.Failure<IReadOnlyList<CartItemModel>>(new Error("Notfound","Cart is empty"));
            }
            var cart = await _cartRepository.GetCartItemAsync(request.cart.Id);
            if(cart == null)
            {
                return Result.Failure<IReadOnlyList<CartItemModel>>(new Error("Notfound", "Cart is empty"));
            }
            var mapped = _mapper.Map<IReadOnlyList<CartItemModel>>(cart);
            return Result.Success(mapped);
        }
    }
}
