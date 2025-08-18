using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IMapper mapper, IProductRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request);
            await _repo.UpdateAsync(entity);
            return Unit.Value;
        }

        
    }
}
