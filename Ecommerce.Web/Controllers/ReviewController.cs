using Ecommerce.Web.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewClient _reviewClient;

        public ReviewController(IReviewClient reviewClient)
        {
            _reviewClient = reviewClient;
        }

        [AllowAnonymous]
        [HttpGet("reviews/product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            var result = await _reviewClient.GetReviewsByProductIdAsync(productId);
            return Json(result);
        }

        [Authorize]
        [HttpPost("reviews/create")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
        {
            var result = await _reviewClient.CreateReviewAsync(request.ProductId, request.Content, request.Rating);
            return Json(result);
        }

        [Authorize]
        [HttpDelete("reviews/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await _reviewClient.DeleteReviewAsync(id);
            return Json(result);
        }
    }

    public class CreateReviewRequest
    {
        public int ProductId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
