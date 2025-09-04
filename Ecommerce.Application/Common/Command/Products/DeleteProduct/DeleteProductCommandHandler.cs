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

namespace Ecommerce.Application.Common.Command.Products.DeleteProduct
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IProductRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(IMapper mapper, IProductRepository repo, IUnitOfWork uow)
        {
            _mapper = mapper;
            _repo = repo;
            _uow = uow;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if(request.id <= 0)
            {
                return Result.Failure(Error.NullValue);
            }
            var deleted = await _repo.Delete(request.id);
            if (!deleted)
            {
                return Result.Failure(new Error("Not Found", $"Can not delete product with id {request.id} cause its not found"));
            }
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
