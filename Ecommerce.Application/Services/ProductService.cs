using AutoMapper;
using Ecommerce.Application.Interfaces.Base;
using Ecommerce.Application.Interfaces.Core;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Core
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IMapper mapper, IProductRepository repository)
        {
            _productRepository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetProductByName(string productName)
        {
            var item = await _productRepository.GetProductByNameAsync(productName);
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(item);
            return mapped;
        }

        public async Task<IEnumerable<ProductModel>> GetProductByCategory(int categoryId)
        {
            var entity = await _productRepository.GetProductByCategoryAsync(categoryId);
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(entity);
            return mapped;
        }

        public async Task<ProductModel> GetProductModelById(int ProductModelId)
        {

            var item = await _productRepository.GetByIdASync(ProductModelId);
            var mapped = _mapper.Map<ProductModel>(item);
            return mapped;
        }

        public async Task<IEnumerable<ProductModel>> GetProductModelByName(string ProductModelName)
        {
            var item = await _productRepository.GetProductByNameAsync(ProductModelName);
            if (item == null)
            {
                throw new ApplicationException($"{ProductModelName} not found!");
            }
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(item);
            return mapped;
        }

        public async Task<IEnumerable<ProductModel>> GetProductModelList()
        {
            var ProductModels = await _productRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(ProductModels);
            return mapped;
        }

        public async Task UpdateAsync(ProductModel model)
        {
            var entity = _mapper.Map<Product>(model);
            await _productRepository.UpdateAsync(entity);
        }

        public Task<ProductModel> GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<ProductModel>> GetAllAsync()
        {
            var entity = await _productRepository.GetAllAsync();
            var mapped = _mapper.Map<IReadOnlyList<ProductModel>>(entity);
            return mapped;
        }


        public async Task<ProductModel> Create(ProductModel model)
        {
            var entity = _mapper.Map<Product>(model);
            var item = await _productRepository.AddAsync(entity);
            var mapped = _mapper.Map<ProductModel>(item);
            return mapped;
        }

        public async Task Update(ProductModel model)
        {
            var entity = _mapper.Map<Product>(model);
            await _productRepository.UpdateAsync(entity);
        }

        public async Task Delete(ProductModel model)
        {
            var entity = _mapper.Map<Product>(model);
            await _productRepository.DeleteAsync(entity);
        }

        public async Task<IEnumerable<ProductModel>> GetProductList()
        {
            var list = await _productRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(list);
            return mapped;
        }
    }
}
