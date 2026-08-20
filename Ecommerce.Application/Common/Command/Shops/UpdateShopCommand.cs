using AutoMapper;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Shops
{
    public sealed record UpdateShopCommand(string UserId, int ShopId, string Name) : IRequest<Result<ShopModel>>
    {
    }
    public sealed class UpdateShopHandler : IRequestHandler<UpdateShopCommand, Result<ShopModel>>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateShopHandler(IUnitOfWork uow, IShopRepository shopRepository, IMapper mapper)
        {
            _uow = uow;
            _shopRepository = shopRepository;
            _mapper = mapper;
        }

        public async Task<Result<ShopModel>> Handle(UpdateShopCommand request, CancellationToken cancellationToken)
        {
            var shop = await _shopRepository.GetByIdAsync(request.ShopId);
            if (shop == null)
            {
                return Result.Failure<ShopModel>(new Error("404", "Không tìm thấy cửa hàng nào"));
            }
            shop.Update(request.Name);
            await _shopRepository.Update(shop);
            await _uow.SaveChangesAsync(cancellationToken);

            var model = _mapper.Map<ShopModel>(shop);
            return Result.Success(model);
        }
    }
}
