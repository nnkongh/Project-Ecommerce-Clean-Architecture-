using Ecommerce.Web.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentClient _commentClient;

        public CommentController(ICommentClient commentClient)
        {
            _commentClient = commentClient;
        }

        [AllowAnonymous]
        [HttpGet("comments/product/{productId}")]
        public async Task<IActionResult> GetCommentsByProductId(int productId)
        {
            var result = await _commentClient.GetCommentsByProductIdAsync(productId);
            return Json(result);
        }

        [Authorize]
        [HttpPost("comments/create")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var result = await _commentClient.CreateCommentAsync(request.ProductId, request.Content);
            return Json(result);
        }

        [Authorize]
        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _commentClient.DeleteCommentAsync(id);
            return Json(result);
        }
    }

    public class CreateCommentRequest
    {
        public int ProductId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
