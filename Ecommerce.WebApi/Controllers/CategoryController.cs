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
    //[Authorize]
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
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.Error);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : NotFound(new {message = $"Category with id {id} not found"});
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequest model)
        {
            var command = new UpdateCategoryCommand(id,model);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : NotFound(new {message = $"Category with id {id} not found"});
        }
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
        {
            var query = new GetAllCategoriesQueries();
            var result = await Sender.Send(query);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategoryById(int id)
        {
            var query = new GetCategoryByIdQueries(id);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = $"Category with id {id} not found" });
        }
    }
}
