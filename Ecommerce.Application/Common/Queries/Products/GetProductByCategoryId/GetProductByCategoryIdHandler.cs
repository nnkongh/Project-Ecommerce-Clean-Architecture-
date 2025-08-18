using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Specification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products.Queries.Products.GetProductByCategory
{
    public class GetProductByCategoryIdHandler : IRequestHandler<GetProductByCategoryIdQueries, IEnumerable<ProductModel>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        public GetProductByCategoryIdHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> Handle(GetProductByCategoryIdQueries request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetProductByCategoryIdAsync(request.categoryId);
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(list);
            return mapped;
        }
    }
}
