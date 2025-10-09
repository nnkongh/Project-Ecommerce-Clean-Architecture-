using AutoMapper;
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

namespace Ecommerce.Application.Common.Command.Categories.DeleteCategory
{
    public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _uow;


        public DeleteCategoryCommandHandler(ICategoryRepository cartRepo, IUnitOfWork unitOfWork)
        {
            _categoryRepo = cartRepo;
            _uow = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if(request.id <= 0)
            {
                return Result.Failure<Result>(Error.NullValue);
            }
            var category = await _categoryRepo.GetByIdAsync(request.id);
            if (category == null)
            {
                return Result.Failure(new Error("", "Category not found"));
            }
            await _categoryRepo.Delete(category);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
