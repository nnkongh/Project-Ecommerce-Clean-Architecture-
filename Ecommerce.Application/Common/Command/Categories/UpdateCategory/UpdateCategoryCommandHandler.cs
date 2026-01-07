using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Categories.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryModel>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork uow)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Result<CategoryModel>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.update.Name))
            {
                return Result.Failure<CategoryModel>(new Error("", "Category name is required"));
            }
            if (request.id <= 0)
            {
                return Result.Failure<CategoryModel>(new Error("", $"Category with id {request.id}"));
            }
            var category = _mapper.Map<Category>(request.update);
            var updated = await _categoryRepository.UpdatePartialAsync(request.id,
                                                                category,
                                                                x => x.Name);
            var mapped = _mapper.Map<CategoryModel>(updated);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success(mapped);
        }
    }
}
