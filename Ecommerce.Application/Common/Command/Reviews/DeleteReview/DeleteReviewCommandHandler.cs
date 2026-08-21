using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Reviews.DeleteReview
{
    public sealed class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Result>
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IUnitOfWork _uow;

        public DeleteReviewCommandHandler(IReviewRepository reviewRepo, IUnitOfWork uow)
        {
            _reviewRepo = reviewRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepo.GetByIdAsync(request.ReviewId);
            if (review == null)
            {
                return Result.Failure(new Error("", "Đánh giá không tồn tại"));
            }

            if (review.UserId != request.UserId)
            {
                return Result.Failure(new Error("", "Bạn không có quyền xóa đánh giá này"));
            }

            await _reviewRepo.Delete(review);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
