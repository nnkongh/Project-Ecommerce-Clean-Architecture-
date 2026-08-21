using Ecommerce.Application.Common.Command.Reviews.CreateReview;
using Ecommerce.Application.Common.Command.Reviews.DeleteReview;
using Ecommerce.Application.Common.Command.Reviews.UpdateReview;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("reviews")]
    public class ReviewController : ApiController
    {
        public ReviewController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new CreateReviewCommand(request.Rating, request.Content, request.ProductId, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<ReviewModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<ReviewModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new UpdateReviewCommand(id, request.Rating, request.Content, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<ReviewModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<ReviewModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new DeleteReviewCommand(id, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<ReviewModel> { IsSuccess = true })
                : BadRequest(new ApiResponse<ReviewModel> { IsSuccess = false, Error = result.Error });
        }
    }

    public record CreateReviewRequest(int Rating, string Content, int ProductId);
    public record UpdateReviewRequest(int Rating, string Content);
}
