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
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
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
            if(request.id <= request.id)
            {
                return Result.Failure(Error.NullValue);
            }
            var entity = _mapper.Map<Product>(request!.id);
            await _repo.DeleteAsync(entity);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
