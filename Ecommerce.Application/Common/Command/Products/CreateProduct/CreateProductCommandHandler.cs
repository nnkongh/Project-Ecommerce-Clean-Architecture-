using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductModel>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request);
            var item = await _repository.AddAsync(entity);
            var mapped = _mapper.Map<ProductModel>(item);
            return mapped;
        }
    }
}
