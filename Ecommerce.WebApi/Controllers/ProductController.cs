using Ecommerce.Application.Common.Queries.Products.GetAllProducts;
using Ecommerce.Application.Products.Queries.Products.GetProductByCategory;
using Ecommerce.Application.Products.Queries.Products.GetProductById;
using Ecommerce.Application.Products.Queries.Products.GetProductByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    [ApiController]
    [Route("product")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("search-name")]
        public async Task<IActionResult> SearchProductByName(string name)
        {
            var result = await _mediator.Send(new GetProductByNameQueries(name));
            return Ok(result);
        }
        [HttpGet("search-id")]
        public async Task<IActionResult> SearchProductByCategoryId(int id)
        {
            var result = await _mediator.Send(new GetProductByCategoryIdQueries(id));
            return Ok(result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProduct()
        {
            var result = await _mediator.Send(new GetProductByCategoryQueries());
            return Ok(result);
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetAllProductsQueries());
            return Ok(result);
        }

    }
}
