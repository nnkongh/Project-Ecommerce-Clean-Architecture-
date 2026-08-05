using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Comments.DeleteComment
{
    public sealed class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IUnitOfWork _uow;

        public DeleteCommentCommandHandler(ICommentRepository commentRepo, IUnitOfWork uow)
        {
            _commentRepo = commentRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepo.GetByIdAsync(request.CommentId);
            if (comment == null)
            {
                return Result.Failure(new Error("", "Bình luận không tồn tại"));
            }

            if (comment.UserId != request.UserId)
            {
                return Result.Failure(new Error("", "Bạn không có quyền xóa bình luận này"));
            }

            await _commentRepo.Delete(comment);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
