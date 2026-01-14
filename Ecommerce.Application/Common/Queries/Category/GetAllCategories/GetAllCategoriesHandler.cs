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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<IReadOnlyList<CategoryModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync();

            var mapped = _mapper.Map<IReadOnlyList<CategoryModel>>(category);

            return Result.Success(mapped);
        }
    }
}
