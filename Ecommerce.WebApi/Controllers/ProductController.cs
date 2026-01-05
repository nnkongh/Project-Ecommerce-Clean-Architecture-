using Ecommerce.Application.Common.Command.Products.CreateProduct;
using Ecommerce.Application.Common.Command.Products.DeleteProduct;
using Ecommerce.Application.Common.Command.Products.UpdateProduct;
using Ecommerce.Application.Common.Queries.Products.GetAllProducts;
using Ecommerce.Application.Common.Queries.Products.GetProductByCategoryId;
using Ecommerce.Application.Common.Queries.Products.GetProductByName;
using Ecommerce.Application.Common.Queries.Products.GetProductsCategory;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Product;
using Ecommerce.Web.ViewModels.ApiResponse;
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

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> SearchProductByName(string name)
        {
            var query = new GetProductByNameQuery(name);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<IEnumerable<ProductModel>> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<ProductModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpGet("by-category/{id}")]
        public async Task<IActionResult> SearchProductByCategoryId(int id)
        {
            var query = new GetProductByCategoryIdQuery(id);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<IEnumerable<ProductModel>> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<ProductModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProduct()
        {
            var query = new GetProductsByCategoryQuery();
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<IEnumerable<ProductModel>> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<ProductModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllProductsQuery();
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<IEnumerable<ProductModel>> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<ProductModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody]CreateProductRequest request)
        {
            var command = new CreateProductCommand(request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<ProductModel> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<ProductModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody]UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(id,request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<ProductModel> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<ProductModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<ProductModel> { IsSuccess = true})
                                    : BadRequest(new ApiResponse<ProductModel> { IsSuccess = false, Error = result.Error });
        }

    }
}
