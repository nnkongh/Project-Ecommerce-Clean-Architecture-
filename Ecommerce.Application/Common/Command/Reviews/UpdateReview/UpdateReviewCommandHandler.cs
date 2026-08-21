using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Reviews.UpdateReview
{
    public sealed class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Result<ReviewModel>>
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateReviewCommandHandler(IReviewRepository reviewRepo, IUnitOfWork uow, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<ReviewModel>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepo.GetByIdAsync(request.ReviewId);
            if (review == null)
            {
                return Result.Failure<ReviewModel>(new Error("", "Đánh giá không tồn tại"));
            }

            if (review.UserId != request.UserId)
            {
                return Result.Failure<ReviewModel>(new Error("", "Bạn không có quyền sửa đánh giá này"));
            }

            if (request.Rating < 1 || request.Rating > 5)
            {
                return Result.Failure<ReviewModel>(new Error("", "Đánh giá phải trong khoảng từ 1 đến 5 sao"));
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return Result.Failure<ReviewModel>(new Error("", "Nội dung đánh giá không được để trống"));
            }

            review.UpdateReview(request.Rating, request.Content);
            await _uow.SaveChangesAsync(cancellationToken);

            var saved = await _reviewRepo.GetByIdWithUserAsync(review.Id);
            var mapped = _mapper.Map<ReviewModel>(saved);
            return Result.Success(mapped);
        }
    }
}
