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

namespace Ecommerce.Application.Products.Queries.Products.GetProductByName
{
    public class GetProductByNameHandler : IRequestHandler<GetProductByNameQueries, IEnumerable<ProductModel>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductByNameHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductModel>> Handle(GetProductByNameQueries request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.name))
            {
                //throw new ValidationException()
            }
            var products = await _repo.GetProductByNameAsync(request.name);
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(products);
            return mapped;
        }
    }
}
