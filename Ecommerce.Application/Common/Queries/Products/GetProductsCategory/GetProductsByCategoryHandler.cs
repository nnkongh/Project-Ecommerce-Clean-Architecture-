﻿using AutoMapper;
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

namespace Ecommerce.Application.Products.Queries.Products.GetProductById
{
    public sealed class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQueries, Result<IEnumerable<ProductModel>>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductsByCategoryHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductModel>>> Handle(GetProductsByCategoryQueries request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetProductByCategoryAsync();
            if (list is null || !list.Any())
            {
                return Result.Failure<IEnumerable<ProductModel>>(Error.NullValue);
            }
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(list);
            return Result.Success(mapped);
        }
    }
}
