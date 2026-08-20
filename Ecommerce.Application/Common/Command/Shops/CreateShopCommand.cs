using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain;
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
    public sealed record CreateShopCommand(string UserId, string ShopName) : IRequest<Result<ShopModel>>
    {
    }



    internal sealed class CreateShopHandler : IRequestHandler<CreateShopCommand, Result<ShopModel>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;

        public CreateShopHandler(IShopRepository shopRepository, IUnitOfWork uow, IMapper mapper)
        {
            _shopRepository = shopRepository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<ShopModel>> Handle(CreateShopCommand request, CancellationToken cancellationToken)
        {
            var user = await _shopRepository.GetByUserIdAsync(request.UserId);
            if (user != null)
            {
                return Result.Failure<ShopModel>(new Error("400", "Một người chỉ được sở hữu một cửa hàng"));
            }

            var shop = Shop.Create(request.ShopName, request.UserId);

            await _shopRepository.AddAsync(shop);
            await _uow.SaveChangesAsync(cancellationToken);

            var model = _mapper.Map<ShopModel>(shop); 
            return Result.Success(model);

        }
    }

    public class ShopModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AddressModel Address { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt {get;set;}
        public IReadOnlyList<ProductModel> ProductModels { get; set; }
        public string ShopName { get; set; }
    }
}
