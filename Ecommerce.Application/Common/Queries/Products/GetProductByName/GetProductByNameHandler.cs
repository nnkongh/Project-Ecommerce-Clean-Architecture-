using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products.Queries.Products.GetProductByName
{
    public sealed class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, Result<IEnumerable<ProductModel>>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductByNameHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<ProductModel>>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            if (request.name is null)
            {
                return Result.Failure<IEnumerable<ProductModel>>(Error.NullValue);
            }
            var list = await _repo.GetProductByNameAsync(request.name);
            if (list is null || !list.Any())
            {
                return Result.Success(Enumerable.Empty<ProductModel>());
            }
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(list);
            return Result.Success(mapped);
        }
    }
}
