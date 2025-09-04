using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.UpdateProduct
{
    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IMapper mapper, IProductRepository repo, IUnitOfWork uow)
        {
            _mapper = mapper;
            _repo = repo;
            _uow = uow;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.update.Name))
            {
                return Result.Failure(new Error("","Name can not be null"));
            }
            if(request.update.Price <= 0)
            {
                return Result.Failure(new Error("", "Price must be greater than zero"));
            }
            var entity = _mapper.Map<Product>(request.update);
            var updated = await _repo.UpdateAsync(request.id,entity,
                                                    x => x.Name,
                                                    x => x.Price,
                                                    x => x.Description,
                                                    x => x.ImageUrl);
            if (!updated)
            {
                return Result.Failure(new Error("", "An error occur while processing progress"));
            }
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        
    }
}
