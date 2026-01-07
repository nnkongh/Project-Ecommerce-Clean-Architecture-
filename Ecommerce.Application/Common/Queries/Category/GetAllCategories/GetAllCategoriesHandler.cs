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

namespace Ecommerce.Application.Common.Queries.Category.GetAllCategories
{
    public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<IReadOnlyList<CategoryModel>>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(IMapper mapper, ICategoryRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Result<IReadOnlyList<CategoryModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repo.GetAsync();
            if(categories is null || !categories.Any())
            {
                return Array.Empty<CategoryModel>();
            }
            var mapped = _mapper.Map<IReadOnlyList<CategoryModel>>(categories);
            return Result.Success(mapped);
        }
    }
}
