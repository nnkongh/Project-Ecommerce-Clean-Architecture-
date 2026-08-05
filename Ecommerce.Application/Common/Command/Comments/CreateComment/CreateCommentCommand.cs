using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Comments.CreateComment
{
    public sealed record CreateCommentCommand(string Content, int ProductId, string UserId) : IRequest<Result<CommentModel>>;
}
