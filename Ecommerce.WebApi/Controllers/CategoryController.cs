using Ecommerce.Application.Common.Command.Categories.CreateCategory;
using Ecommerce.Application.Common.Command.Categories.DeleteCategory;
using Ecommerce.Application.Common.Command.Categories.UpdateCategory;
using Ecommerce.Application.Common.Queries.Category.GetAllCategories;
using Ecommerce.Application.Common.Queries.Category.GetCategoryById;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Category;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    [Authorize]
    [Route("categories")]
    public class CategoryController : ApiController
    {
        public CategoryController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest model)
        {
            var command = new CreateCategoryCommand(model);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<CategoryModel> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<CategoryModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<CategoryModel> { IsSuccess = true })
                                    : BadRequest(new ApiResponse<CategoryModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequest model)
        {
            var command = new UpdateCategoryCommand(id,model);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<CategoryModel> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<CategoryModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<IReadOnlyList<CategoryModel>> { IsSuccess = true, Value = result.Value})
                                    : BadRequest(new ApiResponse<IReadOnlyList<CategoryModel>> { IsSuccess = false, Error = result.Error });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<IReadOnlyList<ProductModel>> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<IReadOnlyList<ProductModel>> { IsSuccess = false, Error = result.Error });
        }
    }
}
