using AutoMapper;
using Ecommerce.Application.Common.Command.Orders.CreateOrder;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.DTOs.Product;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.CheckoutCart
{
    public sealed class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand, Result>
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CheckoutCartCommandHandler(ICartRepository cartRepo, IMapper mapper, IMediator mediator)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepo.GetCartWithItemByUserIdAsync(request.userId);
            if(cart == null)
            {
                return Result.Failure<OrderModel>(new Error("", "Cart not found"));
            }
            if(cart.Items == null || cart.Items.Count == 0)
            {
                return Result.Failure<OrderModel>(new Error("", "Cart is empty"));
            }
            var cartDto = _mapper.Map<CartModel>(cart);
            var result = await _mediator.Send(new CreateOrderByCartCommand(cartDto, request.userId), cancellationToken);
            return result.IsSuccess ? Result.Success("Order created successfully") : Result.Failure(new Error("","Can not create order"));

        }
    }
}
