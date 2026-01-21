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

namespace Ecommerce.Application.Common.Queries.Products.GetAllProducts
{
    internal class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<IReadOnlyList<ProductModel>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<ProductModel>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var produtcs = await _productRepository.GetAsync();

            var productModels = _mapper.Map<IReadOnlyList<ProductModel>>(produtcs);

            return Result.Success(productModels);
        }
    }
}
