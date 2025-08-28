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

namespace Ecommerce.Application.Common.Queries.Category.GetAllCategories
{
    public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQueries, IEnumerable<CategoryModel>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(IMapper mapper, ICategoryRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<IEnumerable<CategoryModel>> Handle(GetAllCategoriesQueries request, CancellationToken cancellationToken)
        {
            var categories = await _repo.GetAllAsync();
            if(categories is null || !categories.Any())
            {
                return Enumerable.Empty<CategoryModel>();
            }
            var mapped = _mapper.Map<IEnumerable<CategoryModel>>(categories);
            return mapped;
        }
    }
}
