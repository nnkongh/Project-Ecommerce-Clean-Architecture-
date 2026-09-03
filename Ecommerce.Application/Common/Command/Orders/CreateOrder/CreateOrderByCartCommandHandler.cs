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
        private readonly IUnitOfWork _uow;

        public CreateOrderByCartCommandHandler(IOrderRepository orderRepo, IMapper mapper, IUnitOfWork uow)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Result<OrderModel>> Handle(CreateOrderByCartCommand request, CancellationToken cancellationToken)
        {
            var userInfo = request.order.User.Address;


            var address = Ecommerce.Domain.Models.Address.Create(userInfo!.District!,
                                              userInfo!.City!,
                                              userInfo!.Province,
                                              userInfo!.Street!,
                                              userInfo!.Ward!);

            var user = request.order.User;
            var order = Order.CreateOrder(user.Id, user.UserName, user.PhoneNumber, user.Email, address);

            foreach(var item in request.order.Cart.Items)
            {
                order.AddItem(item.ImageUrl, item.ProductName!, item.ProductId, item.UnitPrice, item.Quantity);
            }
            await _orderRepo.AddAsync(order);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<OrderModel>(order);
            return Result.Success(mapped);
            
        }
    }
}
