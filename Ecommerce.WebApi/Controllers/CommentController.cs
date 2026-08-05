using Ecommerce.Application.Common.Command.Comments.CreateComment;
using Ecommerce.Application.Common.Command.Comments.DeleteComment;
using Ecommerce.Application.Common.Queries.Comment.GetCommentsByProductId;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("comments")]
    public class CommentController : ApiController
    {
        public CommentController(ISender sender) : base(sender)
        {
        }

        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByProductId(int productId)
        {
            var query = new GetCommentsByProductIdQuery(productId);
            var result = await Sender.Send(query);
            return result.IsSuccess
                ? Ok(new ApiResponse<IReadOnlyList<CommentModel>> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<IReadOnlyList<CommentModel>> { IsSuccess = false, Error = result.Error });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new CreateCommentCommand(request.Content, request.ProductId, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<CommentModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<CommentModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new DeleteCommentCommand(id, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<CommentModel> { IsSuccess = true })
                : BadRequest(new ApiResponse<CommentModel> { IsSuccess = false, Error = result.Error });
        }
    }

    public record CreateCommentRequest(string Content, int ProductId);
}
