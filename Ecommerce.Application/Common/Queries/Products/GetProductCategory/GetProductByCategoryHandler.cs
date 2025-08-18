using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Specification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products.Queries.Products.GetProductById
{
    public class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryQueries, IEnumerable<ProductModel>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductByCategoryHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> Handle(GetProductByCategoryQueries request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                //throw new
            }
            var product = await _repo.GetProductByCategoryAsync();
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(product);
            return mapped;
        }
    }
}
