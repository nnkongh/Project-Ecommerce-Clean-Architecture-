using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQueries, IEnumerable<ProductModel>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        public GetAllProductsHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> Handle(GetAllProductsQueries request, CancellationToken cancellationToken)
        {
            var products = await _repo.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(products);
            return mapped;
        }
    }
}
