using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Shops
{
    public sealed record DeleteShopCommand(string UserId, int ShopId) : IRequest<Result>
    {
    }
    public sealed class DeleteShopHandler : IRequestHandler<DeleteShopCommand, Result>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _uow;

        public DeleteShopHandler(IUnitOfWork uow, IShopRepository shopRepository)
        {
            _uow = uow;
            _shopRepository = shopRepository;
        }

        public async Task<Result> Handle(DeleteShopCommand request, CancellationToken cancellationToken)
        {
            var shop = await _shopRepository.GetByIdAsync(request.ShopId);
            if (shop == null)
            {
                return Result.Failure(new Error("404", "Không tìm thấy cửa hàng nào"));
            }
            if(shop.UserId != request.UserId)
            {
                return Result.Failure(new Error("403", "Đây không phải cửa hàng của bạn"));
            }

            await _shopRepository.Delete(shop);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
