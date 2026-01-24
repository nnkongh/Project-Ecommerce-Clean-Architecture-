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

            var order = Order.CreateOrder(request.order.User!.Id,
                                          request.order.User!.UserName,
                                          request.order.User!.PhoneNumber!,
                                          request.order.User!.Email!,
                                          request.order.User!.Address!.City!,
                                          request.order.User!.Address.Ward!,
                                          request.order.User!.Address.Street!,
                                          request.order.User!.Address.District!,
                                          request.order.User!.Address.Province);

            foreach(var item in request.order.Cart.Items)
            {
                order.AddItem(item.ProductId, item.Quantity, item.ImageUrl, item.UnitPrice, item.ProductName!);
            }
            await _orderRepo.AddAsync(order);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<OrderModel>(order);
            return Result.Success(mapped);
            
        }
    }
}
