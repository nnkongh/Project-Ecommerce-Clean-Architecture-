using AutoMapper;
using Ecommerce.Application.DTOs.Product;
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

namespace Ecommerce.Application.Common.Command.Products.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IProductRepository repository, IMapper mapper, IUnitOfWork uow)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if(request.create is null)
            {
                return Result.Failure(Error.NullValue);
            }
            var entity = _mapper.Map<Product>(request);
            var item = await _repository.AddAsync(entity);
            var mapped = _mapper.Map<ProductModel>(item);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
