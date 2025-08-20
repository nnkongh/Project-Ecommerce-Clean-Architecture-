using Ecommerce.Application.Common.Command.Categories.CreateCategory;
using Ecommerce.Application.Common.Command.Categories.DeleteCategory;
using Ecommerce.Application.Common.Command.Categories.UpdateCategory;
using Ecommerce.Application.Common.Queries.Category.GetAllCategories;
using Ecommerce.Application.Common.Queries.Category.GetCategoryById;
using Ecommerce.Application.DTOs.CRUD.Category;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Web.ViewModels;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    [Authorize]
    [Route("category")]
    public class CategoryController : ApiController
    {
        public CategoryController(ISender sender) : base(sender)
        {
        }
        [HttpPost("add")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest model)
        {
            var command = new CreateCategoryCommand(model);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest model)
        {
            var command = new UpdateCategoryCommand(model);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetCategories()
        {
            var query = new GetAllCategoriesQueries();
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var query = new GetCategoryByIdQueries(id);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
    }
}
