using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Comment.GetCommentsByProductId
{
    public sealed record GetCommentsByProductIdQuery(int ProductId) : IRequest<Result<IReadOnlyList<CommentModel>>>;
}
