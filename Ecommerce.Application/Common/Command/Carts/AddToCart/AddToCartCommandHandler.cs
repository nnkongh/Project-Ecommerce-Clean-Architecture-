using AutoMapper;
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

namespace Ecommerce.Application.Common.Command.Carts.AddToCart
{
    public sealed class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public AddToCartCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdASync(request.ProductId);
            if(product == null)
            {
                return Result.Failure(new Error("ProductNotFound", "The specified product does not exist."));
            }
            var cart = await _cartRepository.GetCartByUserIdAsync(request.UserId);
            if(cart == null)
            {
                cart = new Cart
                {
                    UserId = request.UserId,
                };
                await _cartRepository.AddAsync(cart);
            }
            cart.AddItem(request.ProductId, request.Quantity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
