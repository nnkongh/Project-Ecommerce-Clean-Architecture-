using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Reviews.UpdateReview
{
    public sealed record UpdateReviewCommand(int ReviewId, int Rating, string Content, string UserId) : IRequest<Result<ReviewModel>>;
}
