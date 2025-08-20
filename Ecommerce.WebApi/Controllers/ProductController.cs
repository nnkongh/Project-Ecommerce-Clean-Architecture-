using Ecommerce.Application.Common.Command.Products.CreateProduct;
using Ecommerce.Application.Common.Command.Products.DeleteProduct;
using Ecommerce.Application.Common.Command.Products.UpdateProduct;
using Ecommerce.Application.Common.Queries.Products.GetAllProducts;
using Ecommerce.Application.DTOs.CRUD.Product;
using Ecommerce.Application.Products.Queries.Products.GetProductByCategory;
using Ecommerce.Application.Products.Queries.Products.GetProductById;
using Ecommerce.Application.Products.Queries.Products.GetProductByName;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{

    [Route("product")]
    [Authorize]
    public sealed class ProductController : ApiController
    {
        public ProductController(ISender sender) : base(sender)
        {
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> SearchProductByName(string name)
        {
            var query = new GetProductByNameQueries(name);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpGet("by-category/{id}")]
        public async Task<IActionResult> SearchProductByCategoryId(int id)
        {
            var query = new GetProductByCategoryIdQueries(id);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProduct()
        {
            var query = new GetProductByCategoryQueries();
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllProductsQueries();
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody]CreateProductRequest request)
        {
            var command = new CreateProductCommand(request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody]UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }

    }
}
