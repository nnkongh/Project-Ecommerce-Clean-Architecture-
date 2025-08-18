using AutoMapper;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(IMapper mapper, IProductRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
            {
                // throw new
            }
            var entity = _mapper.Map<Product>(request!.id);
            await _repo.DeleteAsync(entity);
            return Unit.Value;
        }
    }
}
