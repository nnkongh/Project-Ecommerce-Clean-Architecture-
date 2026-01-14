using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<ProductModel>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.id < 0)
            {
                return Result.Failure<ProductModel>(Error.NullValue);
            }
            var product = await _productRepository.GetByIdAsync(request.id);
            if(product == null)
            {
                return Result.Failure<ProductModel>(Error.NullValue);
            }
            var mapped = _mapper.Map<ProductModel>(product);

            return Result.Success(mapped);
        }
    }
}
