using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetProductIdByCategory
{
    public class GetProductIdByCategoryHandler : IRequestHandler<GetProductIdByCategoryQueries, ProductModel>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductIdByCategoryHandler(IMapper mapper, IProductRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ProductModel> Handle(GetProductIdByCategoryQueries request, CancellationToken cancellationToken)
        {
            if(request == null)
            {
                // throw new
            }
            var item = await _repo.GetProductByIdWithCategoryAsync(request!.productId);
            var mapped = _mapper.Map<ProductModel>(item);
            return mapped;
        }
    }
}
