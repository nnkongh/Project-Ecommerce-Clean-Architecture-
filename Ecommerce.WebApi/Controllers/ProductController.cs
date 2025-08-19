using Ecommerce.Application.Common.Command.Products.CreateProduct;
using Ecommerce.Application.Common.Command.Products.DeleteProduct;
using Ecommerce.Application.Common.Command.Products.UpdateProduct;
using Ecommerce.Application.Common.Queries.Products.GetAllProducts;
using Ecommerce.Application.DTOs.CRUD;
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

        [HttpGet("search-name")]
        public async Task<IActionResult> SearchProductByName(string name)
        {
            var result = await Sender.Send(new GetProductByNameQueries(name));
            return Ok(result);
        }
        [HttpGet("search-id")]
        public async Task<IActionResult> SearchProductByCategoryId(int id)
        {
            var result = await Sender.Send(new GetProductByCategoryIdQueries(id));
            return Ok(result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProduct()
        {
            var result = await Sender.Send(new GetProductByCategoryQueries());
            return Ok(result);
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await Sender.Send(new GetAllProductsQueries());
            return Ok(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct(CreateProductRequest request)
        {
            var command = new CreateProductCommand(request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }

    }
}
