using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Orders.CreateOrder
{
    public sealed class CreateOrderByCartCommandHandler : IRequestHandler<CreateOrderByCartCommand, Result<OrderModel>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly ICartRepository _cartRepo;

        public CreateOrderByCartCommandHandler(IOrderRepository orderRepo, IMapper mapper, IUnitOfWork uow, IUserRepository userRepo, ICartRepository cartRepo)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _uow = uow;
            _userRepo = userRepo;
            _cartRepo = cartRepo;
        }

        public async Task<Result<OrderModel>> Handle(CreateOrderByCartCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.userId);
            if(user == null)
            {
                return Result.Failure<OrderModel>(new Error("", "User not found"));
            }
            if(user.Address == null)
            {
                return Result.Failure<OrderModel>(new Error("", "User address not found"));
            }
            var cart = await _cartRepo.GetByIdAsync(request.cart.Id);
            if(cart == null)
            {
                return Result.Failure<OrderModel>(new Error("", "Cart not found"));
            }
            var order = Order.CreateOrder(request.userId, user.UserName!, user.Address!);
            foreach(var item in cart.Items)
            {
                order.AddItem(item.ProductId, item.Quantity, item.UnitPrice, item.ProductName!);
            }
            cart.Clear();
            await _cartRepo.Delete(cart);
            await _orderRepo.AddAsync(order);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<OrderModel>(order);
            return Result.Success(mapped);
            
        }
    }
}
