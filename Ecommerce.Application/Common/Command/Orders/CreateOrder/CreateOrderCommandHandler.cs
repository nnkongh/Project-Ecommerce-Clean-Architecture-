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
    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderModel>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CreateOrderCommandHandler(IOrderRepository orderRepo, IProductRepository productRepo, IUserRepository userRepo, IMapper mapper, IUnitOfWork uow)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Result<OrderModel>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetByIdAsync(command.request.ProductId);
            if (product == null)
            {
                return Result.Failure<OrderModel>(new Error("", $"Product not found"));
            }
            var user = await _userRepo.GetByIdAsync(command.userId);
            if (user == null)
            {
                return Result.Failure<OrderModel>(new Error("", "User not found"));
            }
            if(user.Address == null)
            {
                return Result.Failure<OrderModel>(new Error("", "User address not found"));
            }
            var order = Order.CreateOrder(command.userId, user.UserName!, user.Address);
            order.AddItem(product.Id, command.request.Quantity, product.Price, product.Name);
            await _orderRepo.AddAsync(order);
            product.Stock -= command.request.Quantity;
            await _uow.SaveChangesAsync(cancellationToken);
            var orderDto = _mapper.Map<OrderModel>(order);
            return Result.Success(orderDto);

        }
    }
}
