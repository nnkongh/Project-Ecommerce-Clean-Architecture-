using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Reviews.CreateReview
{
    public sealed record CreateReviewCommand(int Rating, string Content, int ProductId, string UserId) : IRequest<Result<ReviewModel>>;
}
