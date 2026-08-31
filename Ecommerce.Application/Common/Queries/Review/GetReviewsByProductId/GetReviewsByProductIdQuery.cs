using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Review.GetReviewsByProductId
{
    public sealed record GetReviewsByProductIdQuery(int ProductId) : IRequest<Result<IReadOnlyList<ReviewModel>>>;
}
