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
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _uow;


        public DeleteCategoryCommandHandler(ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _uow = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if(request.id <= 0)
            {
                return Result.Failure<Result>(Error.NullValue);
            }
            var deleted = await _repository.Delete(request.id);
            if (!deleted)
            {
                return Result.Failure(new Error("404", "Not Found"));
            }
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
