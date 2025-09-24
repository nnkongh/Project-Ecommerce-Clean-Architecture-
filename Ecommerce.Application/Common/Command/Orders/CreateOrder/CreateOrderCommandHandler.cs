using AutoMapper;
using Ecommerce.Application.DTOs.Models;
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
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Orders.CreateOrder
{
    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderModel>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly ICartRepository _cartRepo;

        public CreateOrderCommandHandler(IOrderRepository orderRepo, IMapper mapper, IUnitOfWork uow, IUserRepository userRepo, ICartRepository cartRepo)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _uow = uow;
            _userRepo = userRepo;
            _cartRepo = cartRepo;
        }

        public async Task<Result<OrderModel>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.cart.UserId);
            if(user == null)
            {
                return Result.Failure<OrderModel>(new Error("", "User not found"));
            }
            var cart = await _cartRepo.GetByIdAsync(request.cart.Id);
            if(cart == null)
            {
                return Result.Failure<OrderModel>(new Error("", "Cart is not found"));
            }
            var order = new Order
            {
                CustomerId = user.Id,
                Address = user.Address,
                OrderDate = DateTime.Now,
                CustomerName = user.UserName!,
                OrderStatus = OrderStatus.Pending,
            };
            foreach(var item in request.cart.Items)
            {
                order.AddItem(item.ProductId,item.Quantity ,item.UnitPrice, item.ProductName!);
            }
            cart.Clear();
            await _orderRepo.AddAsync(order);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<OrderModel>(order);
            return Result.Success(mapped);
            
        }
    }
}
