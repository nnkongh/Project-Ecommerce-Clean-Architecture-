using AutoMapper;
using Ecommerce.Domain.DTOs.Product;
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
    public sealed class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand, Result<OrderModel>>
    {
        private readonly ICartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public CheckoutCartCommandHandler(ICartRepository cartRepo, IOrderRepository orderRepo, IUserRepository userRepo, IUnitOfWork uow, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<OrderModel>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.userId);
            if (user == null)
            {
                return Result.Failure<OrderModel>(new Error("", "User not found"));
            }
            var cart = await _cartRepo.GetCartByUserIdAsync(request.userId);
            if(cart == null)
            {
                return Result.Failure<OrderModel>(new Error("", "Cart not found"));
            }
            var orderItem = cart.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.UnitPrice,
                Quantity = x.Quantity,
                
            }).ToList();
            var order = new Order(request.userId, user.Name,orderItem);
            var mapped = _mapper.Map<OrderModel>(order);
            await _orderRepo.AddAsync(order);
            cart.Clear();
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success(mapped);

        }
    }
}
