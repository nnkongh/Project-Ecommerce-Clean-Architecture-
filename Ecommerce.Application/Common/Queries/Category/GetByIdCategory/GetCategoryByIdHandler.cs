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

namespace Ecommerce.Application.Common.Queries.Category.GetByIdCategory
{
    public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryModel>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<CategoryModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.id < 0)
            {
                return Result.Failure<CategoryModel>(Error.NullValue);
            }
            var category = await _categoryRepository.GetByIdAsync(request.id);
            if(category == null)
            {
                return Result.Failure<CategoryModel>(Error.NullValue);
            }
            var mapped = _mapper.Map<CategoryModel>(category);

            return Result.Success(mapped);
        }
    }
}
