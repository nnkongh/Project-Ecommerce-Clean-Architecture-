using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Reviews.DeleteReview
{
    public sealed record DeleteReviewCommand(int ReviewId, string UserId) : IRequest<Result>;
}
