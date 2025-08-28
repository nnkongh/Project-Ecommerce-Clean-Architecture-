using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Category.GetCategoryById
{
    public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQueries, IEnumerable<CategoryModel>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;
        public GetCategoryByIdHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<IEnumerable<CategoryModel>> Handle(GetCategoryByIdQueries request, CancellationToken cancellationToken)
        {
            if(request.id <= 0)
            {
                return Enumerable.Empty<CategoryModel>();
            }
            var category = await _repo.GetByIdASync(request.id);
            if(category is null)
            {
                return Enumerable.Empty<CategoryModel>();
            }
            var mapped = _mapper.Map<IEnumerable<CategoryModel>>(category);
            return mapped;
        }
    }
}
