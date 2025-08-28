﻿using AutoMapper;
using Ecommerce.Application.DTOs.CRUD.Category;
using Ecommerce.Application.DTOs.Product;
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

namespace Ecommerce.Application.Common.Command.Categories.CreateCategory
{
    public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryModel>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CreateCategoryCommandHandler(ICategoryRepository repo, IMapper mapper, IUnitOfWork uow)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Result<CategoryModel>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.create.Name))
            {
                return Result.Failure<CategoryModel>(new Error("", "Category name cannot be empty"));
            }
            var category = _mapper.Map<Category>(request.create);
            var result = await _repo.AddAsync(category);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<CategoryModel>(result);
            return Result.Success(mapped);

        }
    }
}
