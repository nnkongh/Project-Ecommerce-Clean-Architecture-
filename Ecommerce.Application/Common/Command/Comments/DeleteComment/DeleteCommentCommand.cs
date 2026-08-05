using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Comments.DeleteComment
{
    public sealed record DeleteCommentCommand(int CommentId, string UserId) : IRequest<Result>;
}
